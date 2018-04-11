using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.Dao;
using AutoRebaring.Database.BIM_PORTAL;
using AutoRebaring.Others;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoRebaring.Form
{
    /// <summary>
    /// Interaction logic for InputForm.xaml
    /// </summary>
    public partial class InputForm : UserControl
    {
        #region DatabaseDao
        public MacAddressDao MacAddressDao { get; set; } = new MacAddressDao();
        public ProjectDao ProjectDao { get; set; } = new ProjectDao();
        public BIM_PORTALDbContext BIM_PORTALDbContext { get; set; } = new BIM_PORTALDbContext();
        public UserDao UserDao { get; set; } = new UserDao();
        public UserTypeDao UserTypeDao { get; set; } = new UserTypeDao();
        public UserProjectDao UserProjectDao { get; set; } = new UserProjectDao();
        public MarkDao MarkDao { get; set; } = new MarkDao();
        public ElementTypeDao ElementTypeDao { get; set; } = new ElementTypeDao();
        public ElementTypeProjectDao ElementTypeProjectDao { get; set; } = new ElementTypeProjectDao();
        public CoverParametersDao CoverParametersDao { get; set; } = new CoverParametersDao();
        public LockheadParametersDao LockheadParametersDao { get; set; } = new LockheadParametersDao();
        public DevelopmentParametersDao DevelopmentParametersDao { get; set; } = new DevelopmentParametersDao();
        public AnchorParametersDao AnchorParametersDao { get; set; } = new AnchorParametersDao();
        public RebarTypeDao RebarTypeDao { get; set; } = new RebarTypeDao();
        public RebarVericalParametersDao RebarVericalParametersDao { get; set; } = new RebarVericalParametersDao();
        public StandardChosenDao StandardChosenDao { get; set; } = new StandardChosenDao();
        public StandardFitTypeDao StandardFitTypeDao { get; set; } = new StandardFitTypeDao();
        public StandardFitLengthDao StandardFitLengthDao { get; set; } = new StandardFitLengthDao();
        public RebarDesignTypeDao RebarDesignTypeDao { get; set; } = new RebarDesignTypeDao();
        public StirrupFamilyTypeDao StirrupFamilyTypeDao { get; set; } = new StirrupFamilyTypeDao();
        public View3dDao View3dDao { get; set; } = new View3dDao();
        public OtherParameterDao OtherParameterDao { get; set; } = new OtherParameterDao();
        public StirrupFamilyNameDao StirrupFamilyNameDao { get; set; } = new StirrupFamilyNameDao();
        public string Password { get; set; } = null;
        #endregion

        #region DatabaseID
        public long IDUser { get; set; } = -1;
        public long IDProject { get; set; } = -1;
        public long IDUserType { get; set; } = -1;
        public long IDUserProject { get; set; } = -1;
        public long IDMacAddress { get; set; } = -1;
        public long IDMark { get; set; } = -1;
        public long IDElementType { get; set; } = -1;
        public long IDElementTypeProject { get; set; } = -1;
        public long IDCoverParameter { get; set; } = -1;
        public long IDDevelopmentParamter { get; set; } = -1;
        public long IDLockheadParameter { get; set; } = -1;
        public long IDAnchorParameter { get; set; } = -1;
        public long IDStandardType { get; set; } = -1;
        public long IDStirrupType { get; set; } = -1;
        public long IDStandardVerticalParameter { get; set; } = -1;
        public long IDStirrupVerticalParameter { get; set; } = -1;
        public long IDStandardChosen { get; set; } = -1;
        public long IDStandardFitL { get; set; } = -1;
        public long IDStandardFitL2 { get; set; } = -1;
        public long IDStandardFitL3 { get; set; } = -1;
        public long IDImplantL { get; set; } = -1;
        public long IDImplantL2 { get; set; } = -1;
        #endregion

        #region RevitData
        public Document Document { get; set; }
        public Element Element { get; set; }
        #endregion

        #region FormData
        private List<TextBox> txtFitLs;
        private List<TextBox> txtFitL2s;
        private List<TextBox> txtFitL3s;
        private List<TextBox> txtImplantLs;
        private List<TextBox> txtImplantL2s;
        #endregion

        private bool isFirstSetUserName = false;

        public Window Window { get; set; }
        public InputForm(Document doc, Element e)
        {
            #region Get Revit data
            Document = doc;
            Element = e;
            #endregion

            InitializeComponent();

            FirstInitiate();
            InquireDatabase();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            UpdateDatabase();

            Window.Close();
        }
        private void FirstInitiate()
        {
            FirstAddProject();
            FirstAddMacAddress();
            FirstAddUserName();
            FirstAddRebarFitType();
            FirstAddFamilyStirrup();
            FirstAddCheckLevel();
            FirstAddView3d();
        }

        private void FirstAddProject()
        {
            foreach (var item in BIM_PORTALDbContext.Projects)
            {
                ProjectDao.Update(item.SYS_ID, item.Code + "_" + item.Value);
            }

            lblProject.Content = Document.ProjectInformation.Name;
            IDProject = ProjectDao.GetId((string)lblProject.Content);
        }
        private void FirstAddMacAddress()
        {
            string macAddress = ComputerInfo.GetMacAddress();
            MacAddressDao.Update(macAddress);
            IDMacAddress = MacAddressDao.GetId(macAddress);
        }
        private void FirstAddUserName()
        {
            IDUser = UserProjectDao.GetUserId(IDProject, IDMacAddress);
            var user = UserDao.GetUser(IDUser);
            if (user != null)
            {
                txtUserName.Text = user.Username; txtPassword.Password = user.Password; isFirstSetUserName = true;
                CheckUserName();
            }
            else
            {
                ShowGroupBox(false);
            }
        }
        private void FirstAddDevelopmentParameter()
        {
            chkDevErrorInclude.IsChecked = true; chkDevErrorInclude.IsChecked = false;
            chkDevLevelOffInclude.IsChecked = true; chkDevLevelOffInclude.IsChecked = false;
            chkOff.IsChecked = true; chkOff.IsChecked = false;
            chkOffRatio.IsChecked = true; chkOffRatio.IsChecked = false;
            chkOffStir.IsChecked = true; chkOffStir.IsChecked = false;
            chkOffRatioStir.IsChecked = true; chkOffRatioStir.IsChecked = false;
        }
        private void FirstAddRebarFitType()
        {
            txtFitLs = new List<TextBox> { txtFitL0, txtFitL1, txtFitL2, txtFitL3, txtFitL4, txtFitL5, txtFitL6 };
            txtFitL2s = new List<TextBox> { txtFitL20, txtFitL21, txtFitL22, txtFitL23, txtFitL24, txtFitL25, txtFitL26 };
            txtFitL3s = new List<TextBox> { txtFitL30, txtFitL31, txtFitL32, txtFitL33, txtFitL34, txtFitL35, txtFitL36 };
            txtImplantLs = new List<TextBox> { txtImplantL0, txtImplantL1, txtImplantL2, txtImplantL3, txtImplantL4, txtImplantL5, txtImplantL6 };
            txtImplantL2s = new List<TextBox> { txtImplantL20, txtImplantL21, txtImplantL22, txtImplantL23, txtImplantL24, txtImplantL25, txtImplantL26 };
        }
        private void FirstAddFamilyStirrup()
        {
            rbtColumn.IsChecked = true;
            List<RebarShape> rebarShapes = new FilteredElementCollector(Document).OfClass(typeof(RebarShape)).Where(x => x != null).Cast<RebarShape>().ToList();
            foreach (var item in rebarShapes)
            {
                StirrupFamilyNameDao.Update(item.Name);
                cbbFamilyDaiBao.Items.Add(item);
                cbbFamilyDaiBien.Items.Add(item);
                cbbFamilyDaiC.Items.Add(item);
            }
            cbbFamilyDaiBao.DisplayMemberPath = ConstantValue.Name;
            cbbFamilyDaiBien.DisplayMemberPath = ConstantValue.Name;
            cbbFamilyDaiC.DisplayMemberPath = ConstantValue.Name;
        }
        private void FirstAddCheckLevel()
        {
            lblCheckLevel.Content = Element.LookupParameter(Element is Wall ? ConstantValue.StartLevelWall : ConstantValue.StartLevelColumn).AsValueString();
            Element elemType = Document.GetElement(Element.GetTypeId());
            if (Element is Wall)
            {
                lblCheckb1.Content = Element.LookupParameter(ConstantValue.B1Param_Wall).AsDouble() * ConstantValue.feet2MiliMeter;
                lblCheckb2.Content = elemType.LookupParameter(ConstantValue.B2Param_Wall).AsDouble() * ConstantValue.feet2MiliMeter;
            }
            else
            {
                lblCheckb1.Content = elemType.LookupParameter(ConstantValue.B1Param_Column).AsDouble() * ConstantValue.feet2MiliMeter;
                lblCheckb2.Content = elemType.LookupParameter(ConstantValue.B2Param_Column).AsDouble() * ConstantValue.feet2MiliMeter;
            }
        }
        private void FirstAddView3d()
        {
            chkView3d.IsChecked = true; chkView3d.IsChecked = false;

            List<View3D> view3ds = new FilteredElementCollector(Document).OfClass(typeof(View3D)).Where(x => x != null).Cast<View3D>().ToList();
            foreach (var item in view3ds)
            {
                View3dDao.Update(IDProject, item.Name);
                cbbView3d.Items.Add(item);
            }
            cbbView3d.DisplayMemberPath = ConstantValue.Name;
        }

        private void btnCheckUser_Click(object sender, RoutedEventArgs e)
        {
            CheckUserName();
        }
        private void CheckUserName()
        {
            string userName = txtUserName.Text;
            Password = !isFirstSetUserName ? Encrypting.ToSHA256(txtPassword.Password) : txtPassword.Password;
            isFirstSetUserName = false;

            int res = BIM_PORTALDbContext.Login(userName, Password, (int)IDProject);
            switch (res)
            {
                case 0:
                    if (!isFirstSetUserName)
                        MessageBox.Show(ConstantValue.WrongPassword);
                    ShowGroupBox(false);
                    return;
                case 1:
                    MessageBox.Show(ConstantValue.NonAuthorized);
                    return;
                case 2:
                    IDUserType = UserTypeDao.GetId(ConstantValue.User);
                    break;
                case 3:
                    IDUserType = UserTypeDao.GetId(ConstantValue.Admin);
                    break;
            }
            UserDao.Update(userName, Password);
            IDUser = UserDao.GetId(userName);
            UserProjectDao.Update(IDProject, IDUserType, IDUser, IDMacAddress);
            IDUserProject = UserProjectDao.GetId(IDProject, IDUser);

            int status = UserProjectDao.GetStatus(IDUserProject);
            switch (status)
            {
                case -1:
                    MessageBox.Show(ConstantValue.AuthorizationNotExisted);
                    return;
                case 0:
                    MessageBox.Show(ConstantValue.AuthorizationNotActive);
                    return;
            }
            ShowGroupBox(true);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Password = "";
            ShowGroupBox(false);
        }
        private void ShowGroupBox(bool isShow)
        {
            switch (isShow)
            {
                case true:
                    colParamGrb.Visibility = System.Windows.Visibility.Visible;
                    genParamGrb.Visibility = System.Windows.Visibility.Visible;
                    devRebarGrb.Visibility = System.Windows.Visibility.Visible;
                    rebarChosenGrb.Visibility = System.Windows.Visibility.Visible;
                    otherParametersGrb.Visibility = System.Windows.Visibility.Visible;
                    levelTitleGrb.Visibility = System.Windows.Visibility.Visible;
                    rebarDesGrb.Visibility = System.Windows.Visibility.Visible;
                    btnOK.Visibility = System.Windows.Visibility.Visible;
                    btnCheckUser.IsEnabled = false;
                    break;
                case false:
                    colParamGrb.Visibility = System.Windows.Visibility.Collapsed;
                    genParamGrb.Visibility = System.Windows.Visibility.Collapsed;
                    devRebarGrb.Visibility = System.Windows.Visibility.Collapsed;
                    rebarChosenGrb.Visibility = System.Windows.Visibility.Collapsed;
                    otherParametersGrb.Visibility = System.Windows.Visibility.Collapsed;
                    levelTitleGrb.Visibility = System.Windows.Visibility.Collapsed;
                    rebarDesGrb.Visibility = System.Windows.Visibility.Collapsed;
                    btnOK.Visibility = System.Windows.Visibility.Collapsed;
                    btnCheckUser.IsEnabled = true;
                    break;
            }
        }

        private void InquireDatabase()
        {
            InquireMark();
            InquireCoverParameter();
            InquireDevelopmentParameter();
            InquireLockheadParameter();
            InquireAnchorParameter();
            InquireRebarVerticalParameter();
            InquireStandardChosen();
            InquireStandardFitLength();
            InquireFamilyStirrup();
            InquireOtherParameter();
        }
        private void UpdateDatabase()
        {
            UpdateMark();
            UpdateCoverParameter();
            UpdateDevelopmentParameter();
            UpdateLockheadParameter();
            UpdateAnchorParameter();
            UpdateRebarVerticalParameter();
            UpdateStandardChosen();
            UpdateStandardFitLength();
            UpdateFamilyStirrup();
            UpdateOtherParameter();
        }
        private void InquireMark()
        {
            List<string> markNames = MarkDao.GetAllMarkNames(IDProject);

            if (markNames.Count != 0)
            {
                foreach (var item in markNames)
                {
                    cbbMark.Items.Add(item);
                }
                cbbMark.SelectedIndex = 0;
                IDMark = MarkDao.GetId(IDProject, cbbMark.Text);
                var res = ElementTypeProjectDao.GetElementTypeProject(IDMark);
                if (res != null)
                {
                    string type = ElementTypeDao.GetElementType(res.IDElementType);
                    switch (type)
                    {
                        case ConstantValue.Column:
                            rbtColumn.IsChecked = true;
                            break;
                        case ConstantValue.Wall:
                            rbtWall.IsChecked = true;
                            break;
                    }
                }
            }
        }
        private void UpdateMark()
        {
            MarkDao.Update(IDProject, cbbMark.Text);
            IDMark = MarkDao.GetId(IDProject, cbbMark.Text);
            if (rbtColumn.IsChecked.Value)
            {
                IDElementType = ElementTypeDao.GetId(ConstantValue.Column);
            }
            else
            {
                IDElementType = ElementTypeDao.GetId(ConstantValue.Wall);
            }
            ElementTypeProjectDao.Update(IDMark, IDElementType);
        }

        private void InquireCoverParameter()
        {
            IDCoverParameter = CoverParametersDao.GetId(IDProject);
            var res = CoverParametersDao.GetCoverParameter(IDCoverParameter);
            if (res != null)
            {
                txtConcCover.Text = res.ConcreteCover.ToString();
            }
        }
        private void UpdateCoverParameter()
        {
            double d = double.Parse(txtConcCover.Text);
            CoverParametersDao.Update(IDProject, d);
        }
        private void InquireDevelopmentParameter()
        {
            IDDevelopmentParamter = DevelopmentParametersDao.GetId(IDProject);
            var res = DevelopmentParametersDao.GetDevelopmentParameter(IDDevelopmentParamter);
            if (res != null)
            {
                txtDevMulti.Text = res.DevelopmentMultiply.ToString();
                txtDevsDist.Text = res.DevelopmentLengthsDistance.ToString();
                txtDelDevError.Text = res.DeltaDevelopmentError.ToString();
                txtNumDevError.Text = res.NumberDevelopmentError.ToString();
                chkDevErrorInclude.IsChecked = res.DevelopmentErrorInclude;
                txtDevLevelOffAllow.Text = res.DevelopmentLevelOffsetAllowed.ToString();
                chkDevLevelOffInclude.IsChecked = res.DevelopmentLevelOffsetInclude;
                chkReinfStirrInclude.IsChecked = res.ReinforcementStirrupInclude;
            }
        }
        private void UpdateDevelopmentParameter()
        {
            int devMulti = int.Parse(txtDevMulti.Text);
            int devsDist = int.Parse(txtDevsDist.Text);
            double delDevErr = 0;
            try
            {
                delDevErr = double.Parse(txtDelDevError.Text);
            }
            catch { }
            int numDevErr = 0;
            try
            {
                numDevErr = int.Parse(txtNumDevError.Text);
            }
            catch { }
            bool devErrInclude = chkDevErrorInclude.IsChecked.Value;
            double devLevelOffAllow = 0;
            try
            {
                devLevelOffAllow = double.Parse(txtDevLevelOffAllow.Text);
            }
            catch { }
            bool devLevelOffInclude = chkDevLevelOffInclude.IsChecked.Value;
            bool reinStirrInclude = chkReinfStirrInclude.IsChecked.Value;
            DevelopmentParametersDao.Update(IDProject, devMulti, devsDist, delDevErr, numDevErr, devErrInclude, devLevelOffAllow, devLevelOffInclude, reinStirrInclude);
        }
        private void InquireLockheadParameter()
        {
            IDLockheadParameter = LockheadParametersDao.GetId(IDProject);
            var res = LockheadParametersDao.GetLockheadParameter(IDLockheadParameter);
            if (res != null)
            {
                txtShortenLimit.Text = res.ShortenLimit.ToString();
                txtLockHeadMulti.Text = res.LockheadMutiply.ToString();
                txtConcTopCover.Text = res.LockheadConcreteCover.ToString();
                txtCoverTopSmall.Text = res.SmallConcreteCover.ToString();
                txtRatioLH.Text = res.LHRatio.ToString();
            }
        }
        private void UpdateLockheadParameter()
        {
            double shortenLim = double.Parse(txtShortenLimit.Text);
            int lockheadMulti = int.Parse(txtLockHeadMulti.Text);
            double lockheadConcCover = double.Parse(txtConcTopCover.Text);
            double smallConcCover = double.Parse(txtCoverTopSmall.Text);
            double lhRatio = double.Parse(txtRatioLH.Text);
            LockheadParametersDao.Update(IDProject, shortenLim, lockheadMulti, lockheadConcCover, smallConcCover, lhRatio);
        }
        private void InquireAnchorParameter()
        {
            IDAnchorParameter = AnchorParametersDao.GetId(IDProject);
            var res = AnchorParametersDao.GetAnchorParameter(IDAnchorParameter);
            if (res != null)
            {
                txtAncMulti.Text = res.AnchorMultiply.ToString();
            }
        }
        private void UpdateAnchorParameter()
        {
            int anchorMulti = int.Parse(txtAncMulti.Text);
            AnchorParametersDao.Update(IDProject, anchorMulti);
        }
        private void chkDevErrorInclude_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value && IDUserType == UserTypeDao.GetId(ConstantValue.Admin))
            {
                txtDelDevError.IsEnabled = true;
                txtNumDevError.IsEnabled = true;
            }
            else
            {
                txtDelDevError.IsEnabled = false;
                txtNumDevError.IsEnabled = false;
            }
        }
        public void InquireRebarVerticalParameter()
        {
            IDStandardType = RebarTypeDao.GetId(ConstantValue.Standard);
            IDStandardVerticalParameter = RebarVericalParametersDao.GetId(IDProject, IDStandardType);
            var res = RebarVericalParametersDao.GetRebarVerticalParameter(IDStandardVerticalParameter);
            if (res != null)
            {
                txtBotOff.Text = res.BottomOffset.ToString();
                txtBotOffRatio.Text = res.BottomOffsetRatio.ToString();
                txtTopOff.Text = res.TopOffset.ToString();
                txtTopOffRatio.Text = res.TopOffsetRatio.ToString();
                chkOff.IsChecked = res.OffsetInclude;
                chkOffRatio.IsChecked = res.OffsetRatioInclude;
                chkInsideBeam.IsChecked = res.IsInsideBeam;
            }

            IDStirrupType = RebarTypeDao.GetId(ConstantValue.Stirrup);
            IDStirrupVerticalParameter = RebarVericalParametersDao.GetId(IDProject, IDStirrupType);
            res = RebarVericalParametersDao.GetRebarVerticalParameter(IDStirrupVerticalParameter);
            if (res != null)
            {
                txtBotOffStir.Text = res.BottomOffset.ToString();
                txtBotOffRatioStir.Text = res.BottomOffsetRatio.ToString();
                txtTopOffStir.Text = res.TopOffset.ToString();
                txtTopOffRatioStir.Text = res.TopOffsetRatio.ToString();
                chkOffStir.IsChecked = res.OffsetInclude;
                chkOffRatioStir.IsChecked = res.OffsetRatioInclude;
                chkStirInsideBeam.IsChecked = res.IsInsideBeam;
            }
        }
        public void UpdateRebarVerticalParameter()
        {
            double botOff = 0;
            try
            {
                botOff = double.Parse(txtBotOff.Text);
            }
            catch { }
            double botOffRatio = 1;
            try
            {
                botOffRatio = double.Parse(txtBotOffRatio.Text);
            }
            catch { }
            double topOff = 0;
            try
            {
                topOff = double.Parse(txtTopOff.Text);
            }
            catch { }
            double topOffRatio = 1;
            try
            {
                topOffRatio = double.Parse(txtTopOffRatio.Text);
            }
            catch { }
            bool offInclue = chkOff.IsChecked.Value;
            bool offRatioInclude = chkOffRatio.IsChecked.Value;
            bool insideBeam = chkInsideBeam.IsChecked.Value;

            IDStandardType = RebarTypeDao.GetId(ConstantValue.Standard);
            RebarVericalParametersDao.Update(IDProject, IDStandardType, botOff, botOffRatio, topOff, topOffRatio, offInclue, offRatioInclude, insideBeam);

            botOff = 0;
            try
            {
                botOff = double.Parse(txtBotOffStir.Text);
            }
            catch { }
            botOffRatio = 1;
            try
            {
                botOffRatio = double.Parse(txtBotOffRatioStir.Text);
            }
            catch { }
            topOff = 0;
            try
            {
                topOff = double.Parse(txtTopOffStir.Text);
            }
            catch { }
            topOffRatio = 1;
            try
            {
                topOffRatio = double.Parse(txtTopOffRatioStir.Text);
            }
            catch { }
            offInclue = chkOffStir.IsChecked.Value;
            offRatioInclude = chkOffRatioStir.IsChecked.Value;
            insideBeam = chkStirInsideBeam.IsChecked.Value;

            IDStirrupType = RebarTypeDao.GetId(ConstantValue.Stirrup);
            RebarVericalParametersDao.Update(IDProject, IDStirrupType, botOff, botOffRatio, topOff, topOffRatio, offInclue, offRatioInclude, insideBeam);
        }
        private void InquireStandardChosen()
        {
            IDStandardChosen = StandardChosenDao.GetId(IDProject);
            var res = StandardChosenDao.GetStandardChosen(IDStandardChosen);
            if (res != null)
            {
                txtLmax.Text = res.Lmax.ToString();
                txtLmin.Text = res.Lmin.ToString();
                txtStep.Text = res.Step.ToString();
                txtImplantLmax.Text = res.LImplantMax.ToString();
            }
        }
        private void UpdateStandardChosen()
        {
            double lmax = double.Parse(txtLmax.Text);
            double lmin = double.Parse(txtLmin.Text);
            double step = double.Parse(txtStep.Text);
            double implantLmax = double.Parse(txtImplantLmax.Text);
            StandardChosenDao.Update(IDProject, lmax, lmin, step, implantLmax);
        }
        private void InquireStandardFitLength()
        {
            IDStandardFitL = StandardFitTypeDao.GetId(ConstantValue.FitL);
            IDStandardFitL2 = StandardFitTypeDao.GetId(ConstantValue.FitL2);
            IDStandardFitL3 = StandardFitTypeDao.GetId(ConstantValue.FitL3);
            IDImplantL = StandardFitTypeDao.GetId(ConstantValue.ImplantL);
            IDImplantL2 = StandardFitTypeDao.GetId(ConstantValue.ImplantL2);
            for (int i = 0; i < txtFitLs.Count; i++)
            {
                long id = StandardFitLengthDao.GetId(i, IDStandardFitL, IDProject);
                var res = StandardFitLengthDao.GetStandardFitLength(id);
                if (res != null) txtFitLs[i].Text = res.Length.ToString();

                id = StandardFitLengthDao.GetId(i, IDStandardFitL2, IDProject);
                res = StandardFitLengthDao.GetStandardFitLength(id);
                if (res != null) txtFitL2s[i].Text = res.Length.ToString();

                id = StandardFitLengthDao.GetId(i, IDStandardFitL3, IDProject);
                res = StandardFitLengthDao.GetStandardFitLength(id);
                if (res != null) txtFitL3s[i].Text = res.Length.ToString();

                id = StandardFitLengthDao.GetId(i, IDImplantL, IDProject);
                res = StandardFitLengthDao.GetStandardFitLength(id);
                if (res != null) txtImplantLs[i].Text = res.Length.ToString();

                id = StandardFitLengthDao.GetId(i, IDImplantL2, IDProject);
                res = StandardFitLengthDao.GetStandardFitLength(id);
                if (res != null) txtImplantL2s[i].Text = res.Length.ToString();
            }
        }
        private void UpdateStandardFitLength()
        {
            IDStandardFitL = StandardFitTypeDao.GetId(ConstantValue.FitL);
            IDStandardFitL2 = StandardFitTypeDao.GetId(ConstantValue.FitL2);
            IDStandardFitL3 = StandardFitTypeDao.GetId(ConstantValue.FitL3);
            IDImplantL = StandardFitTypeDao.GetId(ConstantValue.ImplantL);
            IDImplantL2 = StandardFitTypeDao.GetId(ConstantValue.ImplantL2);
            for (int i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtFitLs[i].Text);
                }
                catch { }
                StandardFitLengthDao.Update(i, IDStandardFitL, IDProject, l);

                l = 0;
                try
                {
                    l = double.Parse(txtFitL2s[i].Text);
                }
                catch { }
                StandardFitLengthDao.Update(i, IDStandardFitL2, IDProject, l);

                l = 0;
                try
                {
                    l = double.Parse(txtFitL3s[i].Text);
                }
                catch { }
                StandardFitLengthDao.Update(i, IDStandardFitL3, IDProject, l);

                l = 0;
                try
                {
                    l = double.Parse(txtImplantLs[i].Text);
                }
                catch { }
                StandardFitLengthDao.Update(i, IDImplantL, IDProject, l);

                l = 0;
                try
                {
                    l = double.Parse(txtImplantL2s[i].Text);
                }
                catch { }
                StandardFitLengthDao.Update(i, IDImplantL2, IDProject, l);
            }
        }
        private void InquireFamilyStirrup()
        {
            string famCoverStir = "";
            string famEdgeCoverStir = "";
            string famCStir = "";

            long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CoverStirrup);
            long idStirFamType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            var res = StirrupFamilyTypeDao.GetStirrupFamilyType(idStirFamType);
            Database.AutoRebaring.EF.ARStirrupFamilyName stirFamNameRes = null;
            if (res != null)
            {
                stirFamNameRes = StirrupFamilyNameDao.GetStirrupFamilyname(res.IDStirrupFamilyName);
                if (stirFamNameRes != null)
                {
                    famCoverStir = stirFamNameRes.Name;
                }
            }

            idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CStirrup);
            idStirFamType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            res = StirrupFamilyTypeDao.GetStirrupFamilyType(idStirFamType);
            if (res != null)
            {
                stirFamNameRes = StirrupFamilyNameDao.GetStirrupFamilyname(res.IDStirrupFamilyName);
                if (stirFamNameRes != null)
                {
                    famCStir = stirFamNameRes.Name;
                }
            }

            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Wall))
            {
                idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.EdgeCoverStirrup);
                idStirFamType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
                res = StirrupFamilyTypeDao.GetStirrupFamilyType(idStirFamType);
                if (res != null)
                {
                    stirFamNameRes = StirrupFamilyNameDao.GetStirrupFamilyname(res.IDStirrupFamilyName);
                    if (stirFamNameRes != null)
                    {
                        famEdgeCoverStir = stirFamNameRes.Name;
                    }
                }
            }
            for (int i = 0; i < cbbFamilyDaiBao.Items.Count; i++)
            {
                string s = (cbbFamilyDaiBao.Items[i] as RebarShape).Name;
                if (s == famCoverStir) cbbFamilyDaiBao.SelectedIndex = i;
                if (s == famEdgeCoverStir) cbbFamilyDaiBien.SelectedIndex = i;
                if (s == famCStir) cbbFamilyDaiC.SelectedIndex = i;
            }
        }
        private void UpdateFamilyStirrup()
        {
            long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CoverStirrup);
            long idStirFamName = StirrupFamilyNameDao.GetId(cbbFamilyDaiBao.Text);
            StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamName);
            idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CStirrup);
            idStirFamName = StirrupFamilyNameDao.GetId(cbbFamilyDaiC.Text);
            StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamName);
            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Wall))
            {
                idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.EdgeCoverStirrup);
                idStirFamName = StirrupFamilyNameDao.GetId(cbbFamilyDaiBien.Text);
                StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamName);
            }
        }
        private void InquireOtherParameter()
        {
            long idOtherParameter = OtherParameterDao.GetId(IDMark);
            var res = OtherParameterDao.GetOtherParameter(idOtherParameter);
            if (res != null)
            {
                chkView3d.IsChecked = res.View3dInclude;
                var view3dRes = View3dDao.GetView3d(res.IDView3d);
                if (view3dRes != null)
                {
                    for (int i = 0; i < cbbView3d.Items.Count; i++)
                    {

                        string s = (cbbView3d.Items[i] as View3D).Name;
                        if (s == view3dRes.Name)
                        {
                            cbbView3d.SelectedIndex = i; break;
                        }
                    }
                }
                txtPartCount.Text = res.PartCount.ToString();
            }
        }
        private void UpdateOtherParameter()
        {
            bool view3dInclude = chkView3d.IsChecked.Value;
            string view3d = cbbView3d.Text;
            long idView3d = View3dDao.GetId(IDProject, view3d);
            int partCount = int.Parse(txtPartCount.Text);
            OtherParameterDao.Update(IDMark, idView3d, view3dInclude, partCount);
        }
        private void chkDevLevelOffInclude_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value && IDUserType == UserTypeDao.GetId("Admin"))
            {
                txtDevLevelOffAllow.IsEnabled = true;
            }
            else
            {
                txtDevLevelOffAllow.IsEnabled = false;
            }
        }

        private void chkOff_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                chkOff.IsChecked = true;
                chkOff2.IsChecked = true;
                if (IDUserType == UserTypeDao.GetId("Admin"))
                {
                    txtBotOff.IsEnabled = true;
                    txtTopOff.IsEnabled = true;
                }
            }
            else
            {
                chkOff.IsChecked = false;
                chkOff2.IsChecked = false;
                txtBotOff.IsEnabled = false;
                txtTopOff.IsEnabled = false;
            }
        }

        private void chkOffRatio_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                chkOffRatio.IsChecked = true;
                chkOffRatio2.IsChecked = true;
                if (IDUserType == UserTypeDao.GetId("Admin"))
                {
                    txtBotOffRatio.IsEnabled = true;
                    txtTopOffRatio.IsEnabled = true;
                }
            }
            else
            {
                chkOffRatio.IsChecked = false;
                chkOffRatio2.IsChecked = false;
                txtBotOffRatio.IsEnabled = false;
                txtTopOffRatio.IsEnabled = false;
            }
        }

        private void chkOffStir_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                chkOffStir.IsChecked = true;
                chkOffStir2.IsChecked = true;
                if (IDUserType == UserTypeDao.GetId("Admin"))
                {
                    txtBotOffStir.IsEnabled = true;
                    txtTopOffStir.IsEnabled = true;
                }
            }
            else
            {
                chkOffStir.IsChecked = false;
                chkOffStir2.IsChecked = false;
                txtBotOffStir.IsEnabled = false;
                txtTopOffStir.IsEnabled = false;
            }
        }

        private void chkOffRatioStir_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                chkOffRatioStir.IsChecked = true;
                chkOffRatioStir2.IsChecked = true;
                if (IDUserType == UserTypeDao.GetId("Admin"))
                {
                    txtBotOffRatioStir.IsEnabled = true;
                    txtTopOffRatioStir.IsEnabled = true;
                }
            }
            else
            {
                chkOffRatioStir.IsChecked = false;
                chkOffRatioStir2.IsChecked = false;
                txtBotOffRatioStir.IsEnabled = false;
                txtTopOffRatioStir.IsEnabled = false;
            }
        }

        private void rbtColumn_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtColumn.IsChecked.Value)
            {
                lblDaiBien.Visibility = System.Windows.Visibility.Hidden;
                cbbFamilyDaiBien.Visibility = System.Windows.Visibility.Hidden;
                IDElementType = ElementTypeDao.GetId(ConstantValue.Column);
                InquireFamilyStirrup();
            }
            else
            {
                lblDaiBien.Visibility = System.Windows.Visibility.Visible;
                cbbFamilyDaiBien.Visibility = System.Windows.Visibility.Visible;
                IDElementType = ElementTypeDao.GetId(ConstantValue.Wall);
                InquireFamilyStirrup();
            }
        }

        private void chkView3d_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                cbbView3d.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                cbbView3d.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
