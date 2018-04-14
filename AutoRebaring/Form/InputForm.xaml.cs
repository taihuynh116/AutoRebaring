using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using AutoRebaring.Constant;
using AutoRebaring.Database.AutoRebaring.Dao;
using AutoRebaring.Database.AutoRebaring.EF;
using AutoRebaring.Database.BIM_PORTAL;
using AutoRebaring.ElementInfo;
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
    public partial class InputForm : UserControl, IInputForm
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
        public StandardFitLimitDao StandardFitLimitDao { get; set; } = new StandardFitLimitDao();
        public RebarDesignTypeDao RebarDesignTypeDao { get; set; } = new RebarDesignTypeDao();
        public StirrupFamilyTypeDao StirrupFamilyTypeDao { get; set; } = new StirrupFamilyTypeDao();
        public View3dDao View3dDao { get; set; } = new View3dDao();
        public OtherParameterDao OtherParameterDao { get; set; } = new OtherParameterDao();
        public StirrupFamilyNameDao StirrupFamilyNameDao { get; set; } = new StirrupFamilyNameDao();
        public LevelDao LevelDao { get; set; } = new LevelDao();
        public RebarBarTypeDao RebarBarTypeDao { get; set; } = new RebarBarTypeDao();
        public DesignGeneralDao DesignGeneralDao { get; set; } = new DesignGeneralDao();
        public StandardEndTypeDao StandardEndTypeDao { get; set; } = new StandardEndTypeDao();
        public StandardStartZDao StandardStartZDao { get; set; } = new StandardStartZDao();
        public StandardStartZTypeDao StandardStartZTypeDao { get; set; } = new StandardStartZTypeDao();
        public DesignLevelDao DesignLevelDao { get; set; } = new DesignLevelDao();
        public DesignLevelLimitDao DesignLevelLimitDao { get; set; } = new DesignLevelLimitDao();
        public StandardDesignDao StandardDesignDao { get; set; } = new StandardDesignDao();
        public StirrupDesignDao StirrupDesignDao { get; set; } = new StirrupDesignDao();
        public StandardDesignParameterTypeDao StandardDesignParameterTypeDao { get; set; } = new StandardDesignParameterTypeDao();
        public StandardDesignParameterValueDao StandardDesignParameterValueDao { get; set; } = new StandardDesignParameterValueDao();
        public StirrupDesignParameterTypeDao StirrupDesignParameterTypeDao { get; set; } = new StirrupDesignParameterTypeDao();
        public StirrupDesignParameterValueDao StirrupDesignParameterValueDao { get; set; } = new StirrupDesignParameterValueDao();
        public WallParameterDao WallParameterDao { get; set; } = new WallParameterDao();
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
        public long IDDesignGeneral { get; set; } = -1;
        public long IDCoverStirrupFamilyType { get; set; } = -1;
        public long IDEdgeCoverStirrupFamilyType { get; set; } = -1;
        public long IDCStirrupFamilyType { get; set; } = -1;
        public long IDWallParameter { get; set; } = -1;
        public long IDDesignLevelLimit { get; set; } = -1;
        #endregion

        #region RevitData
        public Document Document { get; set; }
        public Element Element { get; set; }
        public List<Level> Levels { get; set; }
        public List<RebarBarType> RebarBarTypes { get; set; }
        #endregion

        #region HandleData
        public ARElementType ElementType { get;set;}
        public ARWallParameter WallParameter { get; set; }
        public ARCoverParameter CoverParameter { get; set; }
        public ARAnchorParameter AnchorParameter { get; set; }
        public ARDevelopmentParameter DevelopmentParameter { get; set; }
        public ARLockheadParameter LockheadParameter { get; set; }
        public List<IDesignInfo> DesignInfos { get; set; }
        #endregion

        #region FormData
        private List<TextBox> txtFitLs;
        private List<TextBox> txtFitL2s;
        private List<TextBox> txtFitL3s;
        private List<TextBox> txtImplantLs;
        private List<TextBox> txtImplantL2s;
        private List<TextBox> txtMetaLevels;
        private List<Label> lblLevels;
        private List<ComboBox> cbbDesLevels;
        private List<ComboBox> cbbDesStandType1s;
        private List<ComboBox> cbbDesStandType2s;
        private List<ComboBox> cbbDesStirType1s;
        private List<ComboBox> cbbDesStirType2s;
        private List<ComboBox> cbbDesStirType3s;
        private List<TextBox> txtDesN1s;
        private List<TextBox> txtDesN2s;
        private List<TextBox> txtDesN3s;
        private List<TextBox> txtDesN4s;
        private List<TextBox> txtDesN5s;
        private List<TextBox> txtDesN6s;
        private List<TextBox> txtDesStirTB1s;
        private List<TextBox> txtDesStirTB2s;
        private List<TextBox> txtDesStirM1s;
        private List<TextBox> txtDesStirM2s;
        private List<Label> txtDesSums;
        private List<Label> txtEdgeDesSums;
        private List<Label> txtMidDesSums;
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
            GetHandleData();
            Window.Close();
        }
        private void FirstInitiate()
        {
            this.Width = 1140; this.Height = 950;
            FirstAddProject();
            FirstAddMacAddress();
            FirstAddUserName();
            FirstAddRebarFitType();
            FirstAddFamilyStirrup();
            FirstAddCheckLevel();
            FirstAddView3d();
            FirstAddLevel();
            FirstAddDesign();
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
            foreach (var item in txtFitLs)
            {
                item.TextChanged += RebarFit1Change;
            }
            foreach (var item in txtFitL2s)
            {
                item.TextChanged += RebarFit2Change;
            }
            foreach (var item in txtFitL3s)
            {
                item.TextChanged += RebarFit3Change;
            }
            foreach (var item in txtImplantLs)
            {
                item.TextChanged += RebarImplant1Change;
            }
            foreach (var item in txtImplantL2s)
            {
                item.TextChanged += RebarImplant2Change;
            }
        }
        private void RebarFit1Change(object sender, TextChangedEventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < txtFitLs.Count; i++)
            {
                txtFitLs[i].Visibility = visibility;
                if (!isSet)
                {
                    double d = 0;
                    try
                    {
                        d = double.Parse(txtFitLs[i].Text);
                    }
                    catch { }
                    if (d == 0)
                    {
                        visibility = System.Windows.Visibility.Collapsed;
                        isSet = true;
                    }
                }
            }
        }
        private void RebarFit2Change(object sender, TextChangedEventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < txtFitL2s.Count; i++)
            {
                txtFitL2s[i].Visibility = visibility;
                if (!isSet)
                {
                    double d = 0;
                    try
                    {
                        d = double.Parse(txtFitL2s[i].Text);
                    }
                    catch { }
                    if (d == 0)
                    {
                        visibility = System.Windows.Visibility.Collapsed;
                        isSet = true;
                    }
                }
            }
        }
        private void RebarFit3Change(object sender, TextChangedEventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < txtFitL3s.Count; i++)
            {
                txtFitL3s[i].Visibility = visibility;
                if (!isSet)
                {
                    double d = 0;
                    try
                    {
                        d = double.Parse(txtFitL3s[i].Text);
                    }
                    catch { }
                    if (d == 0)
                    {
                        visibility = System.Windows.Visibility.Collapsed;
                        isSet = true;
                    }
                }
            }
        }
        private void RebarImplant1Change(object sender, TextChangedEventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < txtImplantLs.Count; i++)
            {
                txtImplantLs[i].Visibility = visibility;
                if (!isSet)
                {
                    double d = 0;
                    try
                    {
                        d = double.Parse(txtImplantLs[i].Text);
                    }
                    catch { }
                    if (d == 0)
                    {
                        visibility = System.Windows.Visibility.Collapsed;
                        isSet = true;
                    }
                }
            }
        }
        private void RebarImplant2Change(object sender, TextChangedEventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < txtImplantL2s.Count; i++)
            {
                txtImplantL2s[i].Visibility = visibility;
                if (!isSet)
                {
                    double d = 0;
                    try
                    {
                        d = double.Parse(txtImplantL2s[i].Text);
                    }
                    catch { }
                    if (d == 0)
                    {
                        visibility = System.Windows.Visibility.Collapsed;
                        isSet = true;
                    }
                }
            }
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
        private void FirstAddLevel()
        {
            Levels = new FilteredElementCollector(Document).OfClass(typeof(Level)).Where(x => x != null).Cast<Level>().ToList();

            txtMetaLevels = new List<TextBox>();
            lblLevels = new List<Label>();
            for (int i = 0; i < Levels.Count; i++)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                sp.HorizontalAlignment = HorizontalAlignment.Left;
                sp.VerticalAlignment = VerticalAlignment.Top;
                Label lb = new Label();
                lb.Content = Levels[i].Name;
                lblLevels.Add(lb);
                sp.Children.Add(lb);

                TextBox txt = new TextBox();
                txtMetaLevels.Add(txt);
                sp.Children.Add(txt);
                spFull.Children.Add(sp);

                LevelDao.Update(IDProject, Levels[i].Name, Levels[i].Elevation * ConstantValue.feet2MiliMeter);
            }
        }
        private void FirstAddDesign()
        {
            rbtLockHead.IsChecked = true;

            cbbDesLevels = new List<ComboBox> { cbbDesLevel0, cbbDesLevel1, cbbDesLevel2, cbbDesLevel3, cbbDesLevel4, cbbDesLevel5, cbbDesLevel6, cbbDesLevel7 };
            foreach (var item in Levels)
            {
                cbbStartLevel.Items.Add(item);
                cbbEndLevel.Items.Add(item);
                foreach (var item2 in cbbDesLevels)
                {
                    item2.Items.Add(item);
                }
            }
            cbbStartLevel.DisplayMemberPath = ConstantValue.Name;
            cbbEndLevel.DisplayMemberPath = ConstantValue.Name;
            foreach (var item in cbbDesLevels)
            {
                item.DisplayMemberPath = ConstantValue.Name;
            }

            cbbDesStandType1s = new List<ComboBox> { cbbDesStandType10, cbbDesStandType11, cbbDesStandType12, cbbDesStandType13, cbbDesStandType14, cbbDesStandType15, cbbDesStandType16, cbbDesStandType17 };
            cbbDesStandType2s = new List<ComboBox> { cbbDesStandType20, cbbDesStandType21, cbbDesStandType22, cbbDesStandType23, cbbDesStandType24, cbbDesStandType25, cbbDesStandType26, cbbDesStandType27 };
            cbbDesStirType1s = new List<ComboBox> { cbbDesStirType10, cbbDesStirType11, cbbDesStirType12, cbbDesStirType13, cbbDesStirType14, cbbDesStirType15, cbbDesStirType16, cbbDesStirType17 };
            cbbDesStirType2s = new List<ComboBox> { cbbDesStirType20, cbbDesStirType21, cbbDesStirType22, cbbDesStirType23, cbbDesStirType24, cbbDesStirType25, cbbDesStirType26, cbbDesStirType27 };
            cbbDesStirType3s = new List<ComboBox> { cbbDesStirType30, cbbDesStirType31, cbbDesStirType32, cbbDesStirType33, cbbDesStirType34, cbbDesStirType35, cbbDesStirType36, cbbDesStirType37 };
            RebarBarTypes = new FilteredElementCollector(Document).OfClass(typeof(RebarBarType)).Where(x => x != null).Cast<RebarBarType>().ToList();
            for (int i = 0; i < cbbDesStandType1s.Count; i++)
            {
                foreach (var item in RebarBarTypes)
                {
                    if (i == 0)
                    {
                        RebarBarTypeDao.Update(item.Name);
                    }
                    cbbDesStandType1s[i].Items.Add(item);
                    cbbDesStandType2s[i].Items.Add(item);
                    cbbDesStirType1s[i].Items.Add(item);
                    cbbDesStirType2s[i].Items.Add(item);
                    cbbDesStirType3s[i].Items.Add(item);
                }
                cbbDesStandType1s[i].DisplayMemberPath = ConstantValue.Name;
                cbbDesStandType2s[i].DisplayMemberPath = ConstantValue.Name;
                cbbDesStirType1s[i].DisplayMemberPath = ConstantValue.Name;
                cbbDesStirType2s[i].DisplayMemberPath = ConstantValue.Name;
                cbbDesStirType3s[i].DisplayMemberPath = ConstantValue.Name;
            }

            txtDesN1s = new List<TextBox> { txtDesN10, txtDesN11, txtDesN12, txtDesN13, txtDesN14, txtDesN15, txtDesN16, txtDesN17 };
            txtDesN2s = new List<TextBox> { txtDesN20, txtDesN21, txtDesN22, txtDesN23, txtDesN24, txtDesN25, txtDesN26, txtDesN27 };
            txtDesN3s = new List<TextBox> { txtDesN30, txtDesN31, txtDesN32, txtDesN33, txtDesN34, txtDesN35, txtDesN36, txtDesN37 };
            txtDesN4s = new List<TextBox> { txtDesN40, txtDesN41, txtDesN42, txtDesN43, txtDesN44, txtDesN45, txtDesN46, txtDesN47 };
            txtDesN5s = new List<TextBox> { txtDesN50, txtDesN51, txtDesN52, txtDesN53, txtDesN54, txtDesN55, txtDesN56, txtDesN57 };
            txtDesN6s = new List<TextBox> { txtDesN60, txtDesN61, txtDesN62, txtDesN63, txtDesN64, txtDesN65, txtDesN66, txtDesN67 };
            txtDesStirTB1s = new List<TextBox> { txtDesStirTB10, txtDesStirTB11, txtDesStirTB12, txtDesStirTB13, txtDesStirTB14, txtDesStirTB15, txtDesStirTB16, txtDesStirTB17 };
            txtDesStirTB2s = new List<TextBox> { txtDesStirTB20, txtDesStirTB21, txtDesStirTB22, txtDesStirTB23, txtDesStirTB24, txtDesStirTB25, txtDesStirTB26, txtDesStirTB27 };
            txtDesStirM1s = new List<TextBox> { txtDesStirM10, txtDesStirM11, txtDesStirM12, txtDesStirM13, txtDesStirM14, txtDesStirM15, txtDesStirM16, txtDesStirM17 };
            txtDesStirM2s = new List<TextBox> { txtDesStirM20, txtDesStirM21, txtDesStirM22, txtDesStirM23, txtDesStirM24, txtDesStirM25, txtDesStirM26, txtDesStirM27 };

            txtEdgeDesSums = new List<Label> {txtEdgeDesSum0, txtEdgeDesSum1, txtEdgeDesSum2, txtEdgeDesSum3, txtEdgeDesSum4, txtEdgeDesSum5, txtEdgeDesSum6, txtEdgeDesSum7 };
            txtMidDesSums = new List<Label> { txtMidDesSum0, txtMidDesSum1, txtMidDesSum2, txtMidDesSum3, txtMidDesSum4, txtMidDesSum5, txtMidDesSum6, txtMidDesSum7 };
            txtDesSums = new List<Label> { txtDesSum0, txtDesSum1, txtDesSum2, txtDesSum3, txtDesSum4, txtDesSum5, txtDesSum6, txtDesSum7 };

            for (int i = 0; i < cbbDesLevels.Count; i++)
            {
                cbbDesLevels[i].SelectionChanged += DesignChange;
                cbbDesStandType1s[i].SelectionChanged += DesignChange;
                cbbDesStandType2s[i].SelectionChanged += DesignChange;
                txtDesN1s[i].TextChanged += DesignChange;
                txtDesN2s[i].TextChanged += DesignChange;
                txtDesN3s[i].TextChanged += DesignChange;
                txtDesN4s[i].TextChanged += DesignChange;
                txtDesN5s[i].TextChanged += DesignChange;
                txtDesN6s[i].TextChanged += DesignChange;
                cbbDesStirType1s[i].SelectionChanged += DesignChange;
                cbbDesStirType2s[i].SelectionChanged += DesignChange;
                cbbDesStirType3s[i].SelectionChanged += DesignChange;
                txtDesStirTB1s[i].TextChanged += DesignChange;
                txtDesStirTB2s[i].TextChanged += DesignChange;
                txtDesStirM1s[i].TextChanged += DesignChange;
                txtDesStirM2s[i].TextChanged += DesignChange;
            }
        }

        private void DesignChange(object sender, EventArgs e)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
            bool isSet = false;
            for (int i = 0; i < cbbDesLevels.Count; i++)
            {
                if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
                {
                    cbbDesLevels[i].Visibility = visibility;
                    cbbDesStandType1s[i].Visibility = visibility;
                    txtDesN1s[i].Visibility = visibility;
                    txtDesN2s[i].Visibility = visibility;
                    txtDesSums[i].Visibility = visibility;
                    cbbDesStirType1s[i].Visibility = visibility;
                    cbbDesStirType3s[i].Visibility = visibility;
                    txtDesStirTB1s[i].Visibility = visibility;
                    txtDesStirTB2s[i].Visibility = visibility;
                    txtDesStirM1s[i].Visibility = visibility;
                    txtDesStirM2s[i].Visibility = visibility;
                    if (!isSet)
                    {
                        int desLev = cbbDesLevels[i].SelectedIndex, desStandType = cbbDesStandType1s[i].SelectedIndex, desStirType1 = cbbDesStirType1s[i].SelectedIndex, desStirType3 = cbbDesStirType3s[i].SelectedIndex;
                        int b1 = 0, b2 = 0; double tb1 = 0, tb2 = 0, m1 = 0, m2 = 0;
                        try
                        {
                            b1 = int.Parse(txtDesN1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            b2 = int.Parse(txtDesN2s[i].Text);
                        }
                        catch { }
                        try
                        {
                            tb1 = double.Parse(txtDesStirTB1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            tb2 = double.Parse(txtDesStirTB2s[i].Text);
                        }
                        catch { }
                        try
                        {
                            m1 = double.Parse(txtDesStirM1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            m2 = double.Parse(txtDesStirM2s[i].Text);
                        }
                        catch { }
                        if (b1 == 0 || b2 == 0 || tb1 == 0 || tb2 == 0 || m1 == 0 || m2 == 0 || desLev == -1 || desStandType == -1 || desStirType1==-1 || desStirType3==-1)
                        {
                            visibility = System.Windows.Visibility.Collapsed;
                            isSet = true;
                        }
                        txtDesSums[i].Content = ((b1 + b2) * 2 - 4).ToString();
                    }
                }
                else
                {
                    cbbDesLevels[i].Visibility = visibility;
                    cbbDesStandType1s[i].Visibility = visibility;
                    cbbDesStandType2s[i].Visibility = visibility;
                    txtDesN1s[i].Visibility = visibility;
                    txtDesN2s[i].Visibility = visibility;
                    txtDesN3s[i].Visibility = visibility;
                    txtDesN4s[i].Visibility = visibility;
                    txtDesN5s[i].Visibility = visibility;
                    txtDesN6s[i].Visibility = visibility;
                    txtEdgeDesSums[i].Visibility = visibility;
                    txtMidDesSums[i].Visibility = visibility;
                    txtDesSums[i].Visibility = visibility;
                    cbbDesStirType1s[i].Visibility = visibility;
                    cbbDesStirType2s[i].Visibility = visibility;
                    cbbDesStirType3s[i].Visibility = visibility;
                    txtDesStirTB1s[i].Visibility = visibility;
                    txtDesStirTB2s[i].Visibility = visibility;
                    txtDesStirM1s[i].Visibility = visibility;
                    txtDesStirM2s[i].Visibility = visibility;
                    if (!isSet)
                    {
                        int desLev = cbbDesLevels[i].SelectedIndex, desStandType1 = cbbDesStandType1s[i].SelectedIndex, desStandType2 = cbbDesStandType2s[i].SelectedIndex;
                        int desStirType1 = cbbDesStirType1s[i].SelectedIndex, desStirType2 = cbbDesStirType2s[i].SelectedIndex, desStirType3 = cbbDesStirType3s[i].SelectedIndex;
                        int ne11 = 0, ne12 = -1, ce12 = -1, ne2 = 0, de2 = -1, nm = 0; double tb1 = 0, tb2 = 0, m1 = 0, m2 = 0;
                        try
                        {
                            ne11 = int.Parse(txtDesN1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            ne12 = int.Parse(txtDesN2s[i].Text);
                        }
                        catch { }
                        try
                        {
                            ce12 = int.Parse(txtDesN3s[i].Text);
                        }
                        catch { }
                        try
                        {
                            ne2 = int.Parse(txtDesN4s[i].Text);
                        }
                        catch { }
                        try
                        {
                            de2 = int.Parse(txtDesN5s[i].Text);
                        }
                        catch { }
                        try
                        {
                            nm = int.Parse(txtDesN6s[i].Text);
                        }
                        catch { }
                        try
                        {
                            tb1 = double.Parse(txtDesStirTB1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            tb2 = double.Parse(txtDesStirTB2s[i].Text);
                        }
                        catch { }
                        try
                        {
                            m1 = double.Parse(txtDesStirM1s[i].Text);
                        }
                        catch { }
                        try
                        {
                            m2 = double.Parse(txtDesStirM2s[i].Text);
                        }
                        catch { }
                        if (ne11 == 0 || ne12 == -1 || ce12 == -1 || ne2 == 0 || de2 == -1 || tb1 == 0 || tb2 == 0 || m1 == 0 || m2 == 0 || 
                            desLev == -1 || desStandType1 == -1 || desStandType2 == -1 || desStirType1 == -1 || desStirType2 == -1 || desStirType3 == -1)
                        {
                            visibility = System.Windows.Visibility.Collapsed;
                            isSet = true;
                        }
                        int edgeSum = ne11 * 2 + (ne12 - 2) * ce12 + (ne2 - 2) * (de2 + 1);
                        int midSum = nm * 2;
                        int Sum = edgeSum * 2 + midSum;
                        txtEdgeDesSums[i].Content = edgeSum.ToString();
                        txtMidDesSums[i].Content = midSum.ToString();
                        txtDesSums[i].Content = Sum.ToString();
                    }
                }
            }
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
            InquireLevel();
            InquireDesignGeneral();
            InquireDesignDetail();
            InquireWallParameter();
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
            UpdateLevel();
            UpdateDesignGeneral();
            UpdateDesignDetail();
            UpdateWallParameter();
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
                    var elemTypeRes = ElementTypeDao.GetElementType(res.IDElementType);
                    if (elemTypeRes != null)
                    {
                        string type = elemTypeRes.Type;
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

            long idStandFitLimitL = StandardFitLimitDao.GetId(IDProject, IDStandardFitL);
            var standFitLimRes = StandardFitLimitDao.GetStandardFitLimit(idStandFitLimitL);
            if (standFitLimRes != null)
            {
                for (int i = 0; i < standFitLimRes.Limit; i++)
                {
                    long id = StandardFitLengthDao.GetId(i, IDStandardFitL, IDProject);
                    var res = StandardFitLengthDao.GetStandardFitLength(id);
                    if (res != null) txtFitLs[i].Text = res.Length.ToString();
                }
            }

            idStandFitLimitL = StandardFitLimitDao.GetId(IDProject, IDStandardFitL2);
            standFitLimRes = StandardFitLimitDao.GetStandardFitLimit(idStandFitLimitL);
            if (standFitLimRes != null)
            {
                for (int i = 0; i < standFitLimRes.Limit; i++)
                {
                    long id = StandardFitLengthDao.GetId(i, IDStandardFitL2, IDProject);
                    var res = StandardFitLengthDao.GetStandardFitLength(id);
                    if (res != null) txtFitL2s[i].Text = res.Length.ToString();
                }
            }

            idStandFitLimitL = StandardFitLimitDao.GetId(IDProject, IDStandardFitL3);
            standFitLimRes = StandardFitLimitDao.GetStandardFitLimit(idStandFitLimitL);
            if (standFitLimRes != null)
            {
                for (int i = 0; i < standFitLimRes.Limit; i++)
                {
                    long id = StandardFitLengthDao.GetId(i, IDStandardFitL3, IDProject);
                    var res = StandardFitLengthDao.GetStandardFitLength(id);
                    if (res != null) txtFitL3s[i].Text = res.Length.ToString();
                }
            }

            idStandFitLimitL = StandardFitLimitDao.GetId(IDProject, IDImplantL);
            standFitLimRes = StandardFitLimitDao.GetStandardFitLimit(idStandFitLimitL);
            if (standFitLimRes != null)
            {
                for (int i = 0; i < standFitLimRes.Limit; i++)
                {
                    long id = StandardFitLengthDao.GetId(i, IDImplantL, IDProject);
                    var res = StandardFitLengthDao.GetStandardFitLength(id);
                    if (res != null) txtImplantLs[i].Text = res.Length.ToString();
                }
            }

            idStandFitLimitL = StandardFitLimitDao.GetId(IDProject, IDImplantL2);
            standFitLimRes = StandardFitLimitDao.GetStandardFitLimit(idStandFitLimitL);
            if (standFitLimRes != null)
            {
                for (int i = 0; i < standFitLimRes.Limit; i++)
                {
                    long id = StandardFitLengthDao.GetId(i, IDImplantL2, IDProject);
                    var res = StandardFitLengthDao.GetStandardFitLength(id);
                    if (res != null) txtImplantL2s[i].Text = res.Length.ToString();
                }
            }
        }
        private void UpdateStandardFitLength()
        {
            IDStandardFitL = StandardFitTypeDao.GetId(ConstantValue.FitL);
            IDStandardFitL2 = StandardFitTypeDao.GetId(ConstantValue.FitL2);
            IDStandardFitL3 = StandardFitTypeDao.GetId(ConstantValue.FitL3);
            IDImplantL = StandardFitTypeDao.GetId(ConstantValue.ImplantL);
            IDImplantL2 = StandardFitTypeDao.GetId(ConstantValue.ImplantL2);

            int i = 0;
            for (i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtFitLs[i].Text);
                }
                catch { }
                if (l == 0)
                {
                    break;
                }
                StandardFitLengthDao.Update(i, IDStandardFitL, IDProject, l);
            }
            StandardFitLimitDao.Update(IDProject, IDStandardFitL, i);

            for (i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtFitL2s[i].Text);
                }
                catch { }
                if (l == 0)
                {
                    break;
                }
                StandardFitLengthDao.Update(i, IDStandardFitL2, IDProject, l);
            }
            StandardFitLimitDao.Update(IDProject, IDStandardFitL2, i);

            for (i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtFitL3s[i].Text);
                }
                catch { }
                if (l == 0)
                {
                    break;
                }
                StandardFitLengthDao.Update(i, IDStandardFitL3, IDProject, l);
            }
            StandardFitLimitDao.Update(IDProject, IDStandardFitL3, i);

            for (i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtImplantLs[i].Text);
                }
                catch { }
                if (l == 0)
                {
                    break;
                }
                StandardFitLengthDao.Update(i, IDImplantL, IDProject, l);
            }
            StandardFitLimitDao.Update(IDProject, IDImplantL, i);

            for (i = 0; i < txtFitLs.Count; i++)
            {
                double l = 0;
                try
                {
                    l = double.Parse(txtImplantL2s[i].Text);
                }
                catch { }
                if (l == 0)
                {
                    break;
                }
                StandardFitLengthDao.Update(i, IDImplantL2, IDProject, l);
            }
            StandardFitLimitDao.Update(IDProject, IDImplantL2, i);
        }
        private void InquireFamilyStirrup()
        {
            string famCoverStir = "";
            string famEdgeCoverStir = "";
            string famCStir = "";

            long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CoverStirrup);
            IDCoverStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            var res = StirrupFamilyTypeDao.GetStirrupFamilyType(IDCoverStirrupFamilyType);
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
            IDCStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            res = StirrupFamilyTypeDao.GetStirrupFamilyType(IDCStirrupFamilyType);
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
                IDEdgeCoverStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
                res = StirrupFamilyTypeDao.GetStirrupFamilyType(IDEdgeCoverStirrupFamilyType);
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
            long idStirFamNam = StirrupFamilyNameDao.GetId(cbbFamilyDaiBao.Text);
            StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamNam);
            IDCoverStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.CStirrup);
            idStirFamNam = StirrupFamilyNameDao.GetId(cbbFamilyDaiC.Text);
            StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamNam);
            IDCStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Wall))
            {
                idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, IDStirrupType, ConstantValue.EdgeCoverStirrup);
                idStirFamNam = StirrupFamilyNameDao.GetId(cbbFamilyDaiBien.Text);
                StirrupFamilyTypeDao.Update(IDProject, idRebarDesType, idStirFamNam);
                IDEdgeCoverStirrupFamilyType = StirrupFamilyTypeDao.GetId(IDProject, idRebarDesType);
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
        private void InquireLevel()
        {
            List<Database.AutoRebaring.EF.ARLevel> inquireLevels = LevelDao.GetLevels(IDProject);
            for (int i = 0; i < Levels.Count; i++)
            {
                foreach (var item in inquireLevels)
                {
                    if (Levels[i].Name == item.Name)
                    {
                        txtMetaLevels[i].Text = item.Title;
                        break;
                    }
                }
            }
        }
        private void UpdateLevel()
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                LevelDao.Update(IDProject, Levels[i].Name, Levels[i].Elevation * ConstantValue.feet2MiliMeter, txtMetaLevels[i].Text);
            }
        }
        private void InquireDesignGeneral()
        {
            IDDesignGeneral = DesignGeneralDao.GetId(IDMark);
            var res = DesignGeneralDao.GetDesignGeneral(IDDesignGeneral);
            if (res != null)
            {
                string startLevel = "";
                string endLevel = "";
                var levelRes = LevelDao.GetLevel(res.IDStartLevel);
                if (levelRes != null)
                {
                    startLevel = levelRes.Name;
                }
                levelRes = LevelDao.GetLevel(res.IDEndLevel);
                if (levelRes != null)
                {
                    endLevel = levelRes.Name;
                }
                for (int i = 0; i < Levels.Count; i++)
                {
                    string s = Levels[i].Name;
                    if (s == startLevel)
                    {
                        cbbStartLevel.SelectedIndex = i;
                    }
                    if (s == endLevel)
                    {
                        cbbEndLevel.SelectedIndex = i;
                    }
                }
                long idStandEndType = res.IDStandardEndType;
                var standEndTypeRes = StandardEndTypeDao.GetStandardEndType(idStandEndType);
                if (standEndTypeRes != null)
                {
                    if (standEndTypeRes.Type == ConstantValue.Lockhead)
                    {
                        rbtLockHead.IsChecked = true;
                    }
                    else
                    {
                        rbtStartRebar.IsChecked = true;
                    }
                }
            }

            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
            {
                long idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Column);
                long idStandStartZ = StandardStartZDao.GetId(IDDesignGeneral, idStandStartZType);
                var standStartZRes = StandardStartZDao.GetStandardStartZ(idStandStartZ);
                if (standStartZRes != null)
                {
                    txtRebarZ11.Text = standStartZRes.Z1.ToString();
                    txtRebarZ12.Text = standStartZRes.Z2.ToString();
                }
            }
            else
            {
                long idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Edge);
                long idStandStartZ = StandardStartZDao.GetId(IDDesignGeneral, idStandStartZType);
                var standStartZRes = StandardStartZDao.GetStandardStartZ(idStandStartZ);
                if (standStartZRes != null)
                {
                    txtRebarZ11.Text = standStartZRes.Z1.ToString();
                    txtRebarZ12.Text = standStartZRes.Z2.ToString();
                }

                idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Middle);
                idStandStartZ = StandardStartZDao.GetId(IDDesignGeneral, idStandStartZType);
                standStartZRes = StandardStartZDao.GetStandardStartZ(idStandStartZ);
                if (standStartZRes != null)
                {
                    txtRebarZ21.Text = standStartZRes.Z1.ToString();
                    txtRebarZ22.Text = standStartZRes.Z2.ToString();
                }
            }
        }
        private void UpdateDesignGeneral()
        {
            long idStartLevel = LevelDao.GetId(IDProject, cbbStartLevel.Text);
            long idEndLevel = LevelDao.GetId(IDProject, cbbEndLevel.Text);
            long idStandEndType = -1;
            if (rbtLockHead.IsChecked.Value)
            {
                idStandEndType = StandardEndTypeDao.GetId(ConstantValue.Lockhead);
            }
            else
            {
                idStandEndType = StandardEndTypeDao.GetId(ConstantValue.Starter);
            }
            DesignGeneralDao.Update(IDMark, idStartLevel, idEndLevel, idStandEndType);
            IDDesignGeneral = DesignGeneralDao.GetId(IDMark);

            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
            {
                double z1 = 0;
                try
                {
                    z1 = double.Parse(txtRebarZ11.Text);
                }
                catch { }
                double z2 = 0;
                try
                {
                    z2 = double.Parse(txtRebarZ12.Text);
                }
                catch { }
                long idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Column);
                StandardStartZDao.Update(IDDesignGeneral, idStandStartZType, z1, z2);
            }
            else
            {
                double z1 = 0;
                try
                {
                    z1 = double.Parse(txtRebarZ11.Text);
                }
                catch { }
                double z2 = 0;
                try
                {
                    z2 = double.Parse(txtRebarZ12.Text);
                }
                catch { }
                long idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Edge);
                StandardStartZDao.Update(IDDesignGeneral, idStandStartZType, z1, z2);

                z1 = 0;
                try
                {
                    z1 = double.Parse(txtRebarZ21.Text);
                }
                catch { }
                z2 = 0;
                try
                {
                    z2 = double.Parse(txtRebarZ22.Text);
                }
                catch { }
                idStandStartZType = StandardStartZTypeDao.GetId(IDElementType, ConstantValue.Middle);
                StandardStartZDao.Update(IDDesignGeneral, idStandStartZType, z1, z2);
            }
        }
        public void InquireDesignDetail()
        {
            IDDesignLevelLimit = DesignLevelLimitDao.GetId(IDMark);
            var desLevelLimRes = DesignLevelLimitDao.GetDesignLevelLimit(IDDesignLevelLimit);
            if (desLevelLimRes != null)
            {
                for (int i = 0; i < desLevelLimRes.Limit; i++)
                {
                    long idDesLevel = DesignLevelDao.GetId(IDMark, i);
                    var desLevelRes = DesignLevelDao.GetDesignLevel(idDesLevel);
                    if (desLevelRes != null)
                    {
                        var levelRes = LevelDao.GetLevel(desLevelRes.IDDesignLevel);
                        if (levelRes != null)
                        {
                            for (int j = 0; j < Levels.Count; j++)
                            {
                                string s = Levels[j].Name;
                                if (s == levelRes.Name)
                                {
                                    cbbDesLevels[i].SelectedIndex = j;
                                    break;
                                }
                            }
                        }
                        if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
                        {
                            string standBarType = ""; string stirBarType1 = ""; string stirBarType2 = "";

                            long idRebarType = RebarTypeDao.GetId(ConstantValue.Standard);
                            long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.Standard);
                            long idStandDes = StandardDesignDao.GetId(idRebarDesType, idDesLevel);
                            var standDesRes = StandardDesignDao.GetStandardDesign(idStandDes);
                            if (standDesRes != null)
                            {
                                var standBarTypeRes = RebarBarTypeDao.GetRebarBarType(standDesRes.IDRebarBarType);
                                if (standBarTypeRes != null)
                                {
                                    standBarType = standBarTypeRes.Type;
                                }
                            }

                            long idStirDes = StirrupDesignDao.GetId(IDCoverStirrupFamilyType, idDesLevel);
                            var stirDesRes = StirrupDesignDao.GetStirrupDesign(idStirDes);
                            if (stirDesRes != null)
                            {
                                var rebarBarTypeRes = RebarBarTypeDao.GetRebarBarType(stirDesRes.IDRebarBarType);
                                if (rebarBarTypeRes != null)
                                {
                                    stirBarType1 = rebarBarTypeRes.Type;
                                }
                            }

                            idStirDes = StirrupDesignDao.GetId(IDCStirrupFamilyType, idDesLevel);
                            stirDesRes = StirrupDesignDao.GetStirrupDesign(idStirDes);
                            if (stirDesRes != null)
                            {
                                var rebarBarTypeRes = RebarBarTypeDao.GetRebarBarType(stirDesRes.IDRebarBarType);
                                if (rebarBarTypeRes != null)
                                {
                                    stirBarType2 = rebarBarTypeRes.Type;
                                }
                            }

                            for (int j = 0; j < RebarBarTypes.Count; j++)
                            {
                                string s = (RebarBarTypes[j] as RebarBarType).Name;
                                if (s == standBarType) cbbDesStandType1s[i].SelectedIndex = j;
                                if (s == stirBarType1) cbbDesStirType1s[i].SelectedIndex = j;
                                if (s == stirBarType2) cbbDesStirType3s[i].SelectedIndex = j;
                            }

                            long idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.B1);
                            long idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            var standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN1s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.B2);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN2s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            long idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB1);
                            long idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            var stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirTB1s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB2);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirTB2s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M1);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirM1s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M2);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirM2s[i].Text = stirDesParamValueRes.Value.ToString();
                            }
                        }
                        else
                        {
                            string standBarType1 = ""; string standBarType2 = ""; string stirBarType1 = ""; string stirBarType2 = ""; string stirBarType3 = "";

                            long idRebarType = RebarTypeDao.GetId(ConstantValue.Standard);
                            long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.EdgeStandard);
                            long idStandDes = StandardDesignDao.GetId(idRebarDesType, idDesLevel);
                            var standDesRes = StandardDesignDao.GetStandardDesign(idStandDes);
                            if (standDesRes != null)
                            {
                                var standBarTypeRes = RebarBarTypeDao.GetRebarBarType(standDesRes.IDRebarBarType);
                                if (standBarTypeRes != null)
                                {
                                    standBarType1 = standBarTypeRes.Type;
                                }
                            }

                            idRebarType = RebarTypeDao.GetId(ConstantValue.Standard);
                            idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.MiddleStandard);
                            idStandDes = StandardDesignDao.GetId(idRebarDesType, idDesLevel);
                            standDesRes = StandardDesignDao.GetStandardDesign(idStandDes);
                            if (standDesRes != null)
                            {
                                var standBarTypeRes = RebarBarTypeDao.GetRebarBarType(standDesRes.IDRebarBarType);
                                if (standBarTypeRes != null)
                                {
                                    standBarType2 = standBarTypeRes.Type;
                                }
                            }

                            long idStirDes = StirrupDesignDao.GetId(IDCoverStirrupFamilyType, idDesLevel);
                            var stirDesRes = StirrupDesignDao.GetStirrupDesign(idStirDes);
                            if (stirDesRes != null)
                            {
                                var rebarBarTypeRes = RebarBarTypeDao.GetRebarBarType(stirDesRes.IDRebarBarType);
                                if (rebarBarTypeRes != null)
                                {
                                    stirBarType1 = rebarBarTypeRes.Type;
                                }
                            }

                            idStirDes = StirrupDesignDao.GetId(IDEdgeCoverStirrupFamilyType, idDesLevel);
                            stirDesRes = StirrupDesignDao.GetStirrupDesign(idStirDes);
                            if (stirDesRes != null)
                            {
                                var rebarBarTypeRes = RebarBarTypeDao.GetRebarBarType(stirDesRes.IDRebarBarType);
                                if (rebarBarTypeRes != null)
                                {
                                    stirBarType2 = rebarBarTypeRes.Type;
                                }
                            }

                            idStirDes = StirrupDesignDao.GetId(IDCStirrupFamilyType, idDesLevel);
                            stirDesRes = StirrupDesignDao.GetStirrupDesign(idStirDes);
                            if (stirDesRes != null)
                            {
                                var rebarBarTypeRes = RebarBarTypeDao.GetRebarBarType(stirDesRes.IDRebarBarType);
                                if (rebarBarTypeRes != null)
                                {
                                    stirBarType3 = rebarBarTypeRes.Type;
                                }
                            }

                            for (int j = 0; j < RebarBarTypes.Count; j++)
                            {
                                string s = (RebarBarTypes[j] as RebarBarType).Name;
                                if (s == standBarType1) cbbDesStandType1s[i].SelectedIndex = j;
                                if (s == standBarType2) cbbDesStandType2s[i].SelectedIndex = j;
                                if (s == stirBarType1) cbbDesStirType1s[i].SelectedIndex = j;
                                if (s == stirBarType2) cbbDesStirType2s[i].SelectedIndex = j;
                                if (s == stirBarType3) cbbDesStirType3s[i].SelectedIndex = j;
                            }

                            long idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE11);
                            long idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            var standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN1s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE12);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN2s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.CE12);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN3s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE2);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN4s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.DE2);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN5s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NM);
                            idStandDesParamValue = StandardDesignParameterValueDao.GetId(idDesLevel, idStandDesParamType);
                            standDesParamValueRes = StandardDesignParameterValueDao.GetStandardDesignParameterValue(idStandDesParamValue);
                            if (standDesParamValueRes != null)
                            {
                                txtDesN6s[i].Text = standDesParamValueRes.Value.ToString();
                            }

                            long idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB1);
                            long idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            var stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirTB1s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB2);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirTB2s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M1);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirM1s[i].Text = stirDesParamValueRes.Value.ToString();
                            }

                            idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M2);
                            idStirDesParamValue = StirrupDesignParameterValueDao.GetId(idDesLevel, idStirDesParamType);
                            stirDesParamValueRes = StirrupDesignParameterValueDao.GetStirrupDesignParameterValue(idStirDesParamValue);
                            if (stirDesParamValueRes != null)
                            {
                                txtDesStirM2s[i].Text = stirDesParamValueRes.Value.ToString();
                            }
                        }
                    }
                }
            }
        }
        public void UpdateDesignDetail()
        {
            int i = -1;
            for (i = 0; i < cbbDesLevels.Count; i++)
            {
                if (cbbDesLevels[i].SelectedIndex == -1 || cbbDesStandType1s[i].SelectedIndex == -1 || cbbDesStirType1s[i].SelectedIndex == -1 || cbbDesStirType3s[i].SelectedIndex == -1)
                    break;
                if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
                {
                    int b1 = 0, b2 = 0; double tb1 = 0, tb2 = 0, m1 = 0, m2 = 0;
                    try
                    {
                        b1 = int.Parse(txtDesN1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        b2 = int.Parse(txtDesN2s[i].Text);
                    }
                    catch { }
                    try
                    {
                        tb1 = double.Parse(txtDesStirTB1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        tb2 = double.Parse(txtDesStirTB2s[i].Text);
                    }
                    catch { }
                    try
                    {
                        m1 = double.Parse(txtDesStirM1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        m2 = double.Parse(txtDesStirM2s[i].Text);
                    }
                    catch { }
                    if (b1 == 0 || b2 == 0 || tb1 == 0 || tb2 == 0 || m1 == 0 || m2 == 0) { break; }
                    long idLevel = LevelDao.GetId(IDProject, cbbDesLevels[i].Text);
                    DesignLevelDao.Update(IDMark, idLevel, i);
                    long idDesLevel = DesignLevelDao.GetId(IDMark, i);

                    long idRebarBarType = RebarBarTypeDao.GetId(cbbDesStandType1s[i].Text);
                    long idRebarType = RebarTypeDao.GetId(ConstantValue.Standard);
                    long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.Standard);
                    StandardDesignDao.Update(idRebarDesType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStirType1s[i].Text);
                    StirrupDesignDao.Update(IDCoverStirrupFamilyType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStirType3s[i].Text);
                    StirrupDesignDao.Update(IDCStirrupFamilyType, idRebarBarType, idDesLevel);

                    long idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.B1);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN1s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.B2);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN2s[i].Text));

                    long idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB1);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirTB1s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB2);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirTB2s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M1);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirM1s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M2);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirM2s[i].Text));
                }
                else
                {
                    if (cbbDesStandType2s[i].SelectedIndex == -1 || cbbDesStirType2s[i].SelectedIndex == -1)
                        break;
                    int ne11 = 0, ne12 = -1, ce12 = -1, ne2 = 0, de2 = -1, nm = 0; double tb1 = 0, tb2 = 0, m1 = 0, m2 = 0;
                    try
                    {
                        ne11 = int.Parse(txtDesN1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        ne12 = int.Parse(txtDesN2s[i].Text);
                    }
                    catch { }
                    try
                    {
                        ce12 = int.Parse(txtDesN3s[i].Text);
                    }
                    catch { }
                    try
                    {
                        ne2 = int.Parse(txtDesN4s[i].Text);
                    }
                    catch { }
                    try
                    {
                        de2 = int.Parse(txtDesN5s[i].Text);
                    }
                    catch { }
                    try
                    {
                        nm = int.Parse(txtDesN6s[i].Text);
                    }
                    catch { }
                    try
                    {
                        tb1 = double.Parse(txtDesStirTB1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        tb2 = double.Parse(txtDesStirTB2s[i].Text);
                    }
                    catch { }
                    try
                    {
                        m1 = double.Parse(txtDesStirM1s[i].Text);
                    }
                    catch { }
                    try
                    {
                        m2 = double.Parse(txtDesStirM2s[i].Text);
                    }
                    catch { }
                    if (ne11 == 0 || ne12 == -1 || ce12 == -1 || ne2 == 0 || de2 == -1 || tb1 == 0 || tb2 == 0 || m1 == 0 || m2 == 0) { break; }

                    long idLevel = LevelDao.GetId(IDProject, cbbDesLevels[i].Text);
                    DesignLevelDao.Update(IDMark, idLevel, i);
                    long idDesLevel = DesignLevelDao.GetId(IDMark, i);

                    long idRebarBarType = RebarBarTypeDao.GetId(cbbDesStandType1s[i].Text);
                    long idRebarType = RebarTypeDao.GetId(ConstantValue.Standard);
                    long idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.EdgeStandard);
                    StandardDesignDao.Update(idRebarDesType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStandType2s[i].Text);
                    idRebarDesType = RebarDesignTypeDao.GetId(IDElementType, idRebarType, ConstantValue.MiddleStandard);
                    StandardDesignDao.Update(idRebarDesType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStirType1s[i].Text);
                    StirrupDesignDao.Update(IDCoverStirrupFamilyType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStirType2s[i].Text);
                    StirrupDesignDao.Update(IDEdgeCoverStirrupFamilyType, idRebarBarType, idDesLevel);

                    idRebarBarType = RebarBarTypeDao.GetId(cbbDesStirType3s[i].Text);
                    StirrupDesignDao.Update(IDCStirrupFamilyType, idRebarBarType, idDesLevel);

                    long idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE11);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN1s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE12);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN2s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.CE12);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN3s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NE2);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN4s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.DE2);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN5s[i].Text));

                    idStandDesParamType = StandardDesignParameterTypeDao.GetId(IDElementType, ConstantValue.NM);
                    StandardDesignParameterValueDao.Update(idDesLevel, idStandDesParamType, int.Parse(txtDesN6s[i].Text));

                    long idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB1);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirTB1s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.TB2);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirTB2s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M1);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirM1s[i].Text));

                    idStirDesParamType = StirrupDesignParameterTypeDao.GetId(IDElementType, ConstantValue.M2);
                    StirrupDesignParameterValueDao.Update(idDesLevel, idStirDesParamType, double.Parse(txtDesStirM2s[i].Text));
                }
            }
            DesignLevelLimitDao.Update(IDMark, i);
            IDDesignLevelLimit = DesignLevelLimitDao.GetId(IDMark);
        }
        public void InquireWallParameter()
        {
            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Wall))
            {
                IDWallParameter = WallParameterDao.GetId(IDMark);
                var res = WallParameterDao.GetWallParameter(IDWallParameter);
                if (res != null)
                {
                    txtEdgeWidth.Text = res.EdgeWidth.ToString();
                    chkEdgeWidth.IsChecked = res.EdgeWidthInclude;
                    txtEdgeRatio.Text = res.EdgeRatio.ToString();
                    chkEdgeRatio.IsChecked = res.EdgeRatioInclude;
                }
            }
        }
        public void UpdateWallParameter()
        {
            if (IDElementType == ElementTypeDao.GetId(ConstantValue.Wall))
            {
                double edgeWidth = 0;
                double edgeRatio = 0;
                try
                {
                    edgeWidth = double.Parse(txtEdgeWidth.Text);
                }
                catch { }
                try
                {
                    edgeRatio = double.Parse(txtEdgeRatio.Text);
                }
                catch { }
                WallParameterDao.Update(IDMark, edgeWidth, chkEdgeWidth.IsChecked.Value, edgeRatio, chkEdgeRatio.IsChecked.Value);
                IDWallParameter = WallParameterDao.GetId(IDMark);
            }
        }
        public void GetHandleData()
        {
            ElementType = ElementTypeDao.GetElementType(IDElementType);
            WallParameter = WallParameterDao.GetWallParameter(IDWallParameter);
            CoverParameter = CoverParametersDao.GetCoverParameter(IDCoverParameter);
            AnchorParameter = AnchorParametersDao.GetAnchorParameter(IDAnchorParameter);
            DevelopmentParameter = DevelopmentParametersDao.GetDevelopmentParameter(IDDevelopmentParamter);
            LockheadParameter = LockheadParametersDao.GetLockheadParameter(IDLockheadParameter);

            DesignInfos = new List<IDesignInfo>();
            RebarHookType hookType = new FilteredElementCollector(Document).OfClass(typeof(RebarHookType)).Where(x => x != null).Cast<RebarHookType>().First();
            
            var res = DesignLevelLimitDao.GetDesignLevelLimit(IDDesignLevelLimit);
            if (res != null)
            {
                int lim = res.Limit;
                for (int i = 0; i < lim; i++)
                {
                    IDesignInfo desInfo = null;
                    Level level;
                    List<RebarBarType> standTypes;
                    List<RebarHookType> hookTypes;
                    List<int> standNumbers;
                    List<RebarBarType> stirrTypes;
                    List<double> stirrTBs;
                    List<double> stirrMs;
                    if (ElementType.Type == ConstantValue.Column)
                    {
                        level = cbbDesLevels[i].SelectedItem as Level;
                        standTypes = new List<RebarBarType> { cbbDesStandType1s[i].SelectedItem as RebarBarType };
                        hookTypes = new List<RebarHookType> { hookType };
                        standNumbers = new List<int> { int.Parse(txtDesN1s[i].Text), int.Parse(txtDesN2s[i].Text) };
                        stirrTypes = new List<RebarBarType> { cbbDesStirType1s[i].SelectedItem as RebarBarType, cbbDesStirType3s[i].SelectedItem as RebarBarType };
                        stirrTBs = new List<double> { double.Parse(txtDesStirTB1s[i].Text) * ConstantValue.milimeter2Feet, double.Parse(txtDesStirTB2s[i].Text) * ConstantValue.milimeter2Feet };
                        stirrMs = new List<double> { double.Parse(txtDesStirM1s[i].Text) * ConstantValue.milimeter2Feet, double.Parse(txtDesStirM2s[i].Text) * ConstantValue.milimeter2Feet };
                        desInfo = new ColumnDesignInfo(level, standTypes, hookTypes, standNumbers, stirrTypes, stirrTBs, stirrMs);
                    }
                    else
                    {
                        level = cbbDesLevels[i].SelectedItem as Level;
                        standTypes = new List<RebarBarType> { cbbDesStandType1s[i].SelectedItem as RebarBarType, cbbDesStandType2s[i].SelectedItem as RebarBarType };
                        hookTypes = new List<RebarHookType> { hookType, hookType };
                        standNumbers = new List<int> { int.Parse(txtDesN1s[i].Text), int.Parse(txtDesN2s[i].Text), int.Parse(txtDesN3s[i].Text), int.Parse(txtDesN4s[i].Text), int.Parse(txtDesN5s[i].Text), int.Parse(txtDesN6s[i].Text) };
                        stirrTypes = new List<RebarBarType> { cbbDesStirType1s[i].SelectedItem as RebarBarType, cbbDesStirType2s[i].SelectedItem as RebarBarType, cbbDesStirType3s[i].SelectedItem as RebarBarType };
                        stirrTBs = new List<double> { double.Parse(txtDesStirTB1s[i].Text) * ConstantValue.milimeter2Feet, double.Parse(txtDesStirTB2s[i].Text) * ConstantValue.milimeter2Feet };
                        stirrMs = new List<double> { double.Parse(txtDesStirM1s[i].Text) * ConstantValue.milimeter2Feet, double.Parse(txtDesStirM2s[i].Text) * ConstantValue.milimeter2Feet };
                        desInfo = new WallDesignInfo(level, standTypes, hookTypes, standNumbers, stirrTypes, stirrTBs, stirrMs);
                    }
                    DesignInfos.Add(desInfo);
                }
            }
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
                spDaiBien.Visibility = System.Windows.Visibility.Collapsed;
                IDElementType = ElementTypeDao.GetId(ConstantValue.Column);
                InquireFamilyStirrup();

                lblEdgeRebarZ.Visibility = System.Windows.Visibility.Collapsed;
                spMidRebarZ.Visibility = System.Windows.Visibility.Collapsed;
                lblStandardType.Visibility = System.Windows.Visibility.Collapsed;
                lblStandardType1.Content = ConstantValue.KieuThepDoc;
                spStandardType2.Visibility = System.Windows.Visibility.Collapsed;
                lblDesN1.Content = ConstantValue.B1;
                lblDesN2.Content = ConstantValue.B2;
                spDesN3.Visibility = System.Windows.Visibility.Collapsed;
                spDesN4.Visibility = System.Windows.Visibility.Collapsed;
                spDesN5.Visibility = System.Windows.Visibility.Collapsed;
                spDesN6.Visibility = System.Windows.Visibility.Collapsed;
                spEdgeSum.Visibility = System.Windows.Visibility.Collapsed;
                spMidSum.Visibility = System.Windows.Visibility.Collapsed;
                spEdgeCoverStir.Visibility = System.Windows.Visibility.Collapsed;

                wallParamGrb.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                spDaiBien.Visibility = System.Windows.Visibility.Visible;
                IDElementType = ElementTypeDao.GetId(ConstantValue.Wall);
                InquireFamilyStirrup();

                lblEdgeRebarZ.Visibility = System.Windows.Visibility.Visible;
                spMidRebarZ.Visibility = System.Windows.Visibility.Visible;
                lblStandardType.Visibility = System.Windows.Visibility.Visible;
                lblStandardType1.Content = ConstantValue.Vungbien;
                spStandardType2.Visibility = System.Windows.Visibility.Visible;
                lblDesN1.Content = ConstantValue.NE11;
                lblDesN2.Content = ConstantValue.NE12;
                spDesN3.Visibility = System.Windows.Visibility.Visible;
                spDesN4.Visibility = System.Windows.Visibility.Visible;
                spDesN5.Visibility = System.Windows.Visibility.Visible;
                spDesN6.Visibility = System.Windows.Visibility.Visible;
                spEdgeSum.Visibility = System.Windows.Visibility.Visible;
                spMidSum.Visibility = System.Windows.Visibility.Visible;
                spEdgeCoverStir.Visibility = System.Windows.Visibility.Visible;

                wallParamGrb.Visibility = System.Windows.Visibility.Visible;
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

        private void chkEdgeWidth_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                txtEdgeWidth.IsEnabled = true;
            }
            else
            {
                txtEdgeWidth.IsEnabled = false;
            }
        }

        private void chkEdgeRatio_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked.Value)
            {
                txtEdgeRatio.IsEnabled = true;
            }
            else
            {
                txtEdgeRatio.IsEnabled = false;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            IDMark = MarkDao.GetId(IDProject, cbbMark.Text);
            InquireOtherParameter();
            IDElementTypeProject = ElementTypeProjectDao.GetId(IDMark);
            var res = ElementTypeProjectDao.GetElementTypeProject(IDMark);
            if (res != null)
            {
                IDElementType = res.IDElementType;
                if (IDElementType == ElementTypeDao.GetId(ConstantValue.Column))
                {
                    rbtColumn.IsChecked = true;
                }
                else
                {
                    rbtWall.IsChecked = true;
                    InquireWallParameter();
                }
                InquireDesignGeneral();
                InquireDesignDetail();
            }
        }
    }
}
