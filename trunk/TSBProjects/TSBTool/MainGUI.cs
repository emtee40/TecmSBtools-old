using System;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TSBTool
{
	/// <summary>
	/// Summary description for MainGUI.
	/// </summary>
	public class MainGUI : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private RichTextBox richTextBox1;
		private System.Windows.Forms.MenuItem menuItem9;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem loadTSBMenuItem;
		private MenuItem tsbSeasonGenMenuItem;
		private MenuItem tsbSeasonGen_optionsMenuItem;
		private Process process;

		private string programExecName = null;
		private string seasonGenOptionFile = null;
		private ITecmoTool tool;
		private System.Windows.Forms.MenuItem LoadDataMenuItem;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem aboutMenuItem;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem viewTSBContentsMenuItem;
		private System.Windows.Forms.Button loadTSBButton;
		private System.Windows.Forms.Button viewContentsBbutton;
		private System.Windows.Forms.Button loadDataButton;
		private System.Windows.Forms.Button saveDataButton;
		private System.Windows.Forms.MenuItem applyToRomMenuItem;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem findMenuItem;
		private System.Windows.Forms.MenuItem findNextMenuItem;
		private System.Windows.Forms.MenuItem findPrevMenuItem;
		private System.Windows.Forms.MenuItem offensivePrefMenuItem;
		private System.Windows.Forms.MenuItem eolMenuItem;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mOffensiveFormationsMenuItem;
		private System.Windows.Forms.MenuItem mPlaybookMenuItem;
		private System.Windows.Forms.ContextMenu mRichTextBoxontextMenu;
		private System.Windows.Forms.MenuItem mCutMenuItem;
		private System.Windows.Forms.MenuItem mCopyMenuItem;
		private System.Windows.Forms.MenuItem mPasteMenuItem;
		private System.Windows.Forms.MenuItem mFintContextMenuItem;
		private System.Windows.Forms.MenuItem mFindNextContextMenuItem;
		private System.Windows.Forms.MenuItem mFindPrevContextMenuItem;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem testScheduleMenuItem;
		private System.Windows.Forms.MenuItem mDeleteCommasMenuItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mDeleteCommasMenuItem2;
		private System.Windows.Forms.MenuItem mSelectAllMenuItem;
		private System.Windows.Forms.MenuItem mEditPlayersMenuItem;
		private System.Windows.Forms.MenuItem mEditPlayersMenuItem1;
		private System.Windows.Forms.MenuItem mEditTeamsMenuItem;
		private System.Windows.Forms.MenuItem mEditTeamsMenuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mChangeFontItem;
		private System.Windows.Forms.MenuItem mColorsMenuItem;
		private System.Windows.Forms.MenuItem mEditColorsMenuItem;
		private System.Windows.Forms.MenuItem mGetLocationsMenuItem;
		//filter="Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"
		//private string nesFilter = "nes files (*.nes)|*.nes|SNES files (*.smc)|*.smc";
		private string nesFilter = "TSB files (*.nes;*.smc)|*.nes;*.smc";

		public MainGUI(string romFileName, string dataFileName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			tool = TecmoToolFactory.GetToolForRom( romFileName );

			if(romFileName != null && romFileName.Length > 0)
			{
				if( tool.Init(romFileName) )
				{
					state2();
					UpdateTitle(romFileName);
				}
				else 
					state1();
			}
			else
				state1();

			if(dataFileName != null && dataFileName.Length > 0)
			{
				this.LoadDataFile(dataFileName);
			}
			try
			{
				if( File.Exists("TSBSeasonGen.exe") && Directory.Exists("NFL_DATA") )
				{
					programExecName = Path.GetFullPath("TSBSeasonGen.exe");
					seasonGenOptionFile = Path.GetFullPath("SeasonGenOptions.txt");

					MenuItem seasonGen = new MenuItem("&TSBSeasonGen");

					tsbSeasonGenMenuItem = new MenuItem("&Year or other arguments");
					tsbSeasonGenMenuItem.Click +=new EventHandler(tsbSeasonGenMenuItem_Click);
					
					tsbSeasonGen_optionsMenuItem = new MenuItem("&Edit SeasonGen options");
					tsbSeasonGen_optionsMenuItem.Click +=new EventHandler(tsbSeasonGen_optionsMenuItem_Click);

					seasonGen.MenuItems.Add( tsbSeasonGenMenuItem );
					seasonGen.MenuItems.Add( tsbSeasonGen_optionsMenuItem);
					mainMenu1.MenuItems.Add(seasonGen);
				}
			}
			catch {}
			/*richTextBox1.Settings.AddKeywords(tool.GetTeams());
			richTextBox1.Settings.AddKeywords(tool.GetPositionNames());
			string[] others = {"TEAM","WEEK","Face","YEAR","at","Schedule"};
			richTextBox1.Settings.AddKeywords(others);
			richTextBox1.Settings.Comment = "^#";
			richTextBox1.CompileKeywords();
			richTextBox1.CompileRegexes();*/
		}

		// used in GetImage method
		private static NumberForm form = null;

		/// <summary>
		/// Get an image.
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static Image GetImage( string file )
		{
			Image ret = null;
			try
			{
				if( form == null )
					form = new NumberForm("");
				System.IO.Stream s =  
					form.GetType().Assembly.GetManifestResourceStream(file);
				if( s != null )
					ret = Image.FromStream(s);
			}
			catch(Exception e )
			{
				MessageBox.Show(e.Message);
			}
			return ret;
		}

		private void state1()
		{
			viewContentsBbutton.Enabled=false;
			viewTSBContentsMenuItem.Enabled=false;
			applyButton.Enabled=false;
			applyToRomMenuItem.Enabled=false;
		}

		private void state2()
		{
			viewContentsBbutton.Enabled=true;
			viewTSBContentsMenuItem.Enabled=true;
			applyButton.Enabled=true;
			applyToRomMenuItem.Enabled=true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainGUI));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.loadTSBMenuItem = new System.Windows.Forms.MenuItem();
			this.LoadDataMenuItem = new System.Windows.Forms.MenuItem();
			this.applyToRomMenuItem = new System.Windows.Forms.MenuItem();
			this.mGetLocationsMenuItem = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.viewTSBContentsMenuItem = new System.Windows.Forms.MenuItem();
			this.testScheduleMenuItem = new System.Windows.Forms.MenuItem();
			this.offensivePrefMenuItem = new System.Windows.Forms.MenuItem();
			this.mOffensiveFormationsMenuItem = new System.Windows.Forms.MenuItem();
			this.mPlaybookMenuItem = new System.Windows.Forms.MenuItem();
			this.mColorsMenuItem = new System.Windows.Forms.MenuItem();
			this.eolMenuItem = new System.Windows.Forms.MenuItem();
			this.mEditPlayersMenuItem1 = new System.Windows.Forms.MenuItem();
			this.mEditTeamsMenuItem2 = new System.Windows.Forms.MenuItem();
			this.mDeleteCommasMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.findMenuItem = new System.Windows.Forms.MenuItem();
			this.findNextMenuItem = new System.Windows.Forms.MenuItem();
			this.findPrevMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.saveDataButton = new System.Windows.Forms.Button();
			this.loadDataButton = new System.Windows.Forms.Button();
			this.viewContentsBbutton = new System.Windows.Forms.Button();
			this.loadTSBButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.mRichTextBoxontextMenu = new System.Windows.Forms.ContextMenu();
			this.mCutMenuItem = new System.Windows.Forms.MenuItem();
			this.mCopyMenuItem = new System.Windows.Forms.MenuItem();
			this.mPasteMenuItem = new System.Windows.Forms.MenuItem();
			this.mSelectAllMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.mFintContextMenuItem = new System.Windows.Forms.MenuItem();
			this.mFindNextContextMenuItem = new System.Windows.Forms.MenuItem();
			this.mFindPrevContextMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mEditPlayersMenuItem = new System.Windows.Forms.MenuItem();
			this.mEditTeamsMenuItem = new System.Windows.Forms.MenuItem();
			this.mEditColorsMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mDeleteCommasMenuItem2 = new System.Windows.Forms.MenuItem();
			this.mChangeFontItem = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem11,
																					  this.menuItem2,
																					  this.menuItem9});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.loadTSBMenuItem,
																					  this.LoadDataMenuItem,
																					  this.applyToRomMenuItem,
																					  this.mGetLocationsMenuItem,
																					  this.exitMenuItem});
			this.menuItem1.Text = "&File";
			// 
			// loadTSBMenuItem
			// 
			this.loadTSBMenuItem.Index = 0;
			this.loadTSBMenuItem.Text = "Load &TSB ROM";
			this.loadTSBMenuItem.Click += new System.EventHandler(this.loadTSBMenuItem_Click);
			// 
			// LoadDataMenuItem
			// 
			this.LoadDataMenuItem.Index = 1;
			this.LoadDataMenuItem.Text = "Load &Data file";
			this.LoadDataMenuItem.Click += new System.EventHandler(this.LoadDataMenuItem_Click);
			// 
			// applyToRomMenuItem
			// 
			this.applyToRomMenuItem.Index = 2;
			this.applyToRomMenuItem.Text = "&Apply To Rom";
			this.applyToRomMenuItem.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// mGetLocationsMenuItem
			// 
			this.mGetLocationsMenuItem.Index = 3;
			this.mGetLocationsMenuItem.Text = "Get &Bytes";
			this.mGetLocationsMenuItem.Click += new System.EventHandler(this.mGetLocationsMenuItem_Click);
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 4;
			this.exitMenuItem.Text = "E&xit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 1;
			this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem5,
																					   this.viewTSBContentsMenuItem,
																					   this.testScheduleMenuItem,
																					   this.offensivePrefMenuItem,
																					   this.mOffensiveFormationsMenuItem,
																					   this.mPlaybookMenuItem,
																					   this.mColorsMenuItem,
																					   this.eolMenuItem,
																					   this.mEditPlayersMenuItem1,
																					   this.mEditTeamsMenuItem2,
																					   this.mDeleteCommasMenuItem});
			this.menuItem11.Text = "&View";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "Number &Guys Tool";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// viewTSBContentsMenuItem
			// 
			this.viewTSBContentsMenuItem.Index = 1;
			this.viewTSBContentsMenuItem.Text = "View TSB contents";
			this.viewTSBContentsMenuItem.Click += new System.EventHandler(this.viewTSBContentsMenuItem_Click);
			// 
			// testScheduleMenuItem
			// 
			this.testScheduleMenuItem.Index = 2;
			this.testScheduleMenuItem.Text = "Show Schedule Only";
			this.testScheduleMenuItem.Click += new System.EventHandler(this.testScheduleMenuItem_Click);
			// 
			// offensivePrefMenuItem
			// 
			this.offensivePrefMenuItem.Checked = true;
			this.offensivePrefMenuItem.Index = 3;
			this.offensivePrefMenuItem.Text = "Show Offensive Team &Preference";
			this.offensivePrefMenuItem.Click += new System.EventHandler(this.offensivePrefMenuItem_Click);
			// 
			// mOffensiveFormationsMenuItem
			// 
			this.mOffensiveFormationsMenuItem.Checked = true;
			this.mOffensiveFormationsMenuItem.Index = 4;
			this.mOffensiveFormationsMenuItem.Text = "Show Offensive Formaions";
			this.mOffensiveFormationsMenuItem.Click += new System.EventHandler(this.mOffensiveFormationsMenuItem_Click);
			// 
			// mPlaybookMenuItem
			// 
			this.mPlaybookMenuItem.Checked = true;
			this.mPlaybookMenuItem.Index = 5;
			this.mPlaybookMenuItem.Text = "Show Playbooks";
			this.mPlaybookMenuItem.Click += new System.EventHandler(this.mPlaybookMenuItem_Click);
			// 
			// mColorsMenuItem
			// 
			this.mColorsMenuItem.Index = 6;
			this.mColorsMenuItem.Text = "Show &Colors";
			this.mColorsMenuItem.Click += new System.EventHandler(this.mColorsMenuItem_Click);
			// 
			// eolMenuItem
			// 
			this.eolMenuItem.Checked = true;
			this.eolMenuItem.Index = 7;
			this.eolMenuItem.Text = "EOL= Windows Style (CR LF)";
			this.eolMenuItem.Click += new System.EventHandler(this.eolMenuItem_Click);
			// 
			// mEditPlayersMenuItem1
			// 
			this.mEditPlayersMenuItem1.Index = 8;
			this.mEditPlayersMenuItem1.Text = "&Edit Players";
			this.mEditPlayersMenuItem1.Click += new System.EventHandler(this.EditPlayers_Click);
			// 
			// mEditTeamsMenuItem2
			// 
			this.mEditTeamsMenuItem2.Index = 9;
			this.mEditTeamsMenuItem2.Text = "Edit &Teams";
			this.mEditTeamsMenuItem2.Click += new System.EventHandler(this.mEditTeamsMenuItem_Click);
			// 
			// mDeleteCommasMenuItem
			// 
			this.mDeleteCommasMenuItem.Index = 10;
			this.mDeleteCommasMenuItem.Text = "&Delete Trailing Commas";
			this.mDeleteCommasMenuItem.Click += new System.EventHandler(this.mDeleteCommasMenuItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.findMenuItem,
																					  this.findNextMenuItem,
																					  this.findPrevMenuItem});
			this.menuItem2.Text = "&Search";
			// 
			// findMenuItem
			// 
			this.findMenuItem.Index = 0;
			this.findMenuItem.Text = "&Find    (Ctrl+F)";
			this.findMenuItem.Click += new System.EventHandler(this.findMenuItem_Click);
			// 
			// findNextMenuItem
			// 
			this.findNextMenuItem.Index = 1;
			this.findNextMenuItem.Text = "Find &Next  (F3)";
			this.findNextMenuItem.Click += new System.EventHandler(this.findNextMenuItem_Click);
			// 
			// findPrevMenuItem
			// 
			this.findPrevMenuItem.Index = 2;
			this.findPrevMenuItem.Text = "Find &Prev  (F2)";
			this.findPrevMenuItem.Click += new System.EventHandler(this.findPrevMenuItem_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 3;
			this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.aboutMenuItem});
			this.menuItem9.Text = "A&bout";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 0;
			this.aboutMenuItem.Text = "About &TSBTool";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.statusBar1);
			this.panel1.Controls.Add(this.saveDataButton);
			this.panel1.Controls.Add(this.loadDataButton);
			this.panel1.Controls.Add(this.viewContentsBbutton);
			this.panel1.Controls.Add(this.loadTSBButton);
			this.panel1.Controls.Add(this.applyButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 486);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(680, 72);
			this.panel1.TabIndex = 0;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 50);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(680, 22);
			this.statusBar1.TabIndex = 5;
			// 
			// saveDataButton
			// 
			this.saveDataButton.Location = new System.Drawing.Point(276, 12);
			this.saveDataButton.Name = "saveDataButton";
			this.saveDataButton.Size = new System.Drawing.Size(128, 32);
			this.saveDataButton.TabIndex = 2;
			this.saveDataButton.Text = "&Save Data";
			this.saveDataButton.Click += new System.EventHandler(this.saveDataButton_Click);
			this.saveDataButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loadTSBButton_KeyDown);
			// 
			// loadDataButton
			// 
			this.loadDataButton.Location = new System.Drawing.Point(406, 12);
			this.loadDataButton.Name = "loadDataButton";
			this.loadDataButton.Size = new System.Drawing.Size(128, 32);
			this.loadDataButton.TabIndex = 3;
			this.loadDataButton.Text = "&Load Data";
			this.loadDataButton.Click += new System.EventHandler(this.LoadDataMenuItem_Click);
			this.loadDataButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loadTSBButton_KeyDown);
			// 
			// viewContentsBbutton
			// 
			this.viewContentsBbutton.Location = new System.Drawing.Point(146, 12);
			this.viewContentsBbutton.Name = "viewContentsBbutton";
			this.viewContentsBbutton.Size = new System.Drawing.Size(128, 32);
			this.viewContentsBbutton.TabIndex = 1;
			this.viewContentsBbutton.Text = "View &Contents";
			this.viewContentsBbutton.Click += new System.EventHandler(this.viewTSBContentsMenuItem_Click);
			this.viewContentsBbutton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loadTSBButton_KeyDown);
			// 
			// loadTSBButton
			// 
			this.loadTSBButton.Location = new System.Drawing.Point(16, 12);
			this.loadTSBButton.Name = "loadTSBButton";
			this.loadTSBButton.Size = new System.Drawing.Size(128, 32);
			this.loadTSBButton.TabIndex = 0;
			this.loadTSBButton.Text = "&Load TSB Rom       (nes or snes TSB1)";
			this.loadTSBButton.Click += new System.EventHandler(this.loadTSBMenuItem_Click);
			this.loadTSBButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loadTSBButton_KeyDown);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(536, 12);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(128, 32);
			this.applyButton.TabIndex = 4;
			this.applyButton.Text = "&Apply to Rom";
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			this.applyButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loadTSBButton_KeyDown);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.richTextBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(680, 486);
			this.panel2.TabIndex = 1;
			// 
			// richTextBox1
			// 
			this.richTextBox1.AcceptsTab = true;
			this.richTextBox1.ContextMenu = this.mRichTextBoxontextMenu;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.richTextBox1.Size = new System.Drawing.Size(680, 486);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
			this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
			// 
			// mRichTextBoxontextMenu
			// 
			this.mRichTextBoxontextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								   this.mCutMenuItem,
																								   this.mCopyMenuItem,
																								   this.mPasteMenuItem,
																								   this.mSelectAllMenuItem,
																								   this.menuItem6,
																								   this.mFintContextMenuItem,
																								   this.mFindNextContextMenuItem,
																								   this.mFindPrevContextMenuItem,
																								   this.menuItem3,
																								   this.mEditPlayersMenuItem,
																								   this.mEditTeamsMenuItem,
																								   this.mEditColorsMenuItem,
																								   this.menuItem4,
																								   this.mDeleteCommasMenuItem2,
																								   this.mChangeFontItem});
			// 
			// mCutMenuItem
			// 
			this.mCutMenuItem.Index = 0;
			this.mCutMenuItem.Text = "C&ut       (Ctrl+X)";
			this.mCutMenuItem.Click += new System.EventHandler(this.mCutMenuItem_Click);
			// 
			// mCopyMenuItem
			// 
			this.mCopyMenuItem.Index = 1;
			this.mCopyMenuItem.Text = "&Copy    (Ctrl+C)";
			this.mCopyMenuItem.Click += new System.EventHandler(this.mCopyMenuItem_Click);
			// 
			// mPasteMenuItem
			// 
			this.mPasteMenuItem.Index = 2;
			this.mPasteMenuItem.Text = "&Paste   (Ctrl+V)";
			this.mPasteMenuItem.Click += new System.EventHandler(this.mPasteMenuItem_Click);
			// 
			// mSelectAllMenuItem
			// 
			this.mSelectAllMenuItem.Index = 3;
			this.mSelectAllMenuItem.Text = "Select &All  (Ctrl+A)";
			this.mSelectAllMenuItem.Click += new System.EventHandler(this.mSelectAllMenuItem_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// mFintContextMenuItem
			// 
			this.mFintContextMenuItem.Index = 5;
			this.mFintContextMenuItem.Text = "&Find          (Ctrl+F)";
			this.mFintContextMenuItem.Click += new System.EventHandler(this.findMenuItem_Click);
			// 
			// mFindNextContextMenuItem
			// 
			this.mFindNextContextMenuItem.Index = 6;
			this.mFindNextContextMenuItem.Text = "Find &Next (F3)";
			this.mFindNextContextMenuItem.Click += new System.EventHandler(this.findNextMenuItem_Click);
			// 
			// mFindPrevContextMenuItem
			// 
			this.mFindPrevContextMenuItem.Index = 7;
			this.mFindPrevContextMenuItem.Text = "Find &Prev (F2)";
			this.mFindPrevContextMenuItem.Click += new System.EventHandler(this.findPrevMenuItem_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 8;
			this.menuItem3.Text = "-";
			// 
			// mEditPlayersMenuItem
			// 
			this.mEditPlayersMenuItem.Index = 9;
			this.mEditPlayersMenuItem.Text = "Edit &Players";
			this.mEditPlayersMenuItem.Click += new System.EventHandler(this.EditPlayers_Click);
			// 
			// mEditTeamsMenuItem
			// 
			this.mEditTeamsMenuItem.Index = 10;
			this.mEditTeamsMenuItem.Text = "Edit &Teams";
			this.mEditTeamsMenuItem.Click += new System.EventHandler(this.mEditTeamsMenuItem_Click);
			// 
			// mEditColorsMenuItem
			// 
			this.mEditColorsMenuItem.Index = 11;
			this.mEditColorsMenuItem.Text = "Edit &Colors";
			this.mEditColorsMenuItem.Click += new System.EventHandler(this.mEditColorsMenuItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 12;
			this.menuItem4.Text = "-";
			// 
			// mDeleteCommasMenuItem2
			// 
			this.mDeleteCommasMenuItem2.Index = 13;
			this.mDeleteCommasMenuItem2.Text = "&Delete trailing commas ";
			this.mDeleteCommasMenuItem2.Click += new System.EventHandler(this.mDeleteCommasMenuItem_Click);
			// 
			// mChangeFontItem
			// 
			this.mChangeFontItem.Index = 14;
			this.mChangeFontItem.Text = "Change &Font (and resize form)";
			this.mChangeFontItem.Click += new System.EventHandler(this.mChangeFontItem_Click);
			// 
			// MainGUI
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 558);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(688, 200);
			this.Name = "MainGUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TSBTool Supreme";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainGUI_Closing);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		//loadTSBMenuItem
		private void loadTSBMenuItem_Click(object sender, System.EventArgs e)
		{
			string filename = GetFileName(nesFilter, false);

			tool = TecmoToolFactory.GetToolForRom( filename );
			if(filename != null && tool != null)
			{
				if( tool.OutputRom != null )
				{
					state2();
					UpdateTitle(filename);
				}
				else if( tool.Init(filename))
				{
					state2();
					UpdateTitle(filename);
				}
				else
					state1();
			}
		}

		private void UpdateTitle(string filename )
		{
			if( filename != null )
			{
				string fn = filename;
				int index = filename.LastIndexOf(Path.DirectorySeparatorChar)+1;
				if( index > 0 )
				{
					fn = filename.Substring(index);
				}
				if( fn.Length > 4 )
					this.Text = string.Format("TSBTool Supreme   '{0}' Loaded", fn);
				string type = "  (normal nes file)";
				if( this.tool is CXRomTSBTool )
					type = "   (32 team ROM file)";
				else if( this.tool is SNES_TecmoTool )
				{
					type = "   (SNES TSB1 ROM file)";
				}
				this.Text += type;
			}
		}

		private void ApplyToRom(string saveToFilename)
		{
			string[] lines = richTextBox1.Lines;
			InputParser parser = new InputParser(tool);
			parser.ProcessLines(lines);
			tool.SaveRom(saveToFilename);
			UpdateTitle(saveToFilename);
		}

		/// <summary>
		/// Returns filename on 'OK' null on 'cancel'.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		private string GetFileName(string filter, bool saveFileDlg)
		{
			string ret=null;
			FileDialog dlg;
			if( saveFileDlg )
			{
				dlg = new SaveFileDialog();
			}
			else
			{
				dlg = new OpenFileDialog();
			}
			dlg.CheckFileExists = false;
			dlg.RestoreDirectory = true;
			//dlg.Filter="nes files (*.nes)|*.nes";
			if(filter != null && filter.Length > 0)
				dlg.Filter = filter;
			if(dlg.ShowDialog() == DialogResult.OK) 
			{
				ret = dlg.FileName;
			}
			return ret;
		}

		private void SetText( string text )
		{
			this.richTextBox1.Text = text;
			richTextBox1.SelectAll();
			richTextBox1.SelectionColor = Color.Black;
			richTextBox1.SelectionLength = 0;
			richTextBox1.SelectionStart = 0;
		}

		private void LoadDataFile(string fileName)
		{
			if(fileName != null && fileName.Length > 0)
			{
				try
				{
					StreamReader sr = new StreamReader(fileName);
					string s = sr.ReadToEnd();
					sr.Close();
					SetText(s);
				}
				catch(Exception ee)
				{ 
					System.Diagnostics.Debug.WriteLine(ee.StackTrace); 
				}
			}
		}

		private void LoadDataMenuItem_Click(object sender, System.EventArgs e)
		{
			string fileName = GetFileName(null,false);
			LoadDataFile(fileName);
		}

		private void applyButton_Click(object sender, System.EventArgs e)
		{
			string filename = GetFileName(nesFilter,true);
			if(filename != null)
				ApplyToRom(filename);
		}

		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			cleanupProcess();
			Application.Exit();
		}

		private void viewTSBContentsMenuItem_Click(object sender, System.EventArgs e)
		{
			tool.ShowOffPref = this.offensivePrefMenuItem.Checked;
			TecmoTool.ShowPlaybook = mPlaybookMenuItem.Checked;
			TecmoTool.ShowTeamFormation = mOffensiveFormationsMenuItem.Checked;

			string msg = 
					"# NEW!!! -> Double click on a team or player to bring up the All new Player/Team editing GUI.\r\n"+
				    "# NEW!!! -> Select (Show Colors) menu Item (under view Menu) to enable listing of team colors.\r\n"+
				    "# NEW!!! -> Double Click on a 'COLORS' line to edit team COLORS.\r\n";
			string text = msg
				+
				
				tool.GetKey()+ tool.GetAll() + tool.GetSchedule();
			SetText(text);
			richTextBox1.SelectionStart = 0;
			richTextBox1.SelectionLength = msg.Length-2;
			richTextBox1.SelectionColor = Color.Magenta;
			richTextBox1.SelectionStart = 0;
			richTextBox1.SelectionLength = 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="data"></param>
		/// <param name="doAppend"></param>
		public void LOG(string fileName, string data)
		{
			try
			{
				StreamWriter sw;
				// the first time we create the file,
				// after that, we append to the file.
				if( logCount == 0 )
					sw = new StreamWriter(fileName);
				else
					sw = new StreamWriter(fileName,true);

				sw.Write( data );
				logCount++;
				sw.Close();
			} 
			catch ( Exception e )
			{ 
				Console.Error.WriteLine("{0}", e.Message);
			} 
		}

		private static int logCount=0;

		private void saveDataButton_Click(object sender, System.EventArgs e)
		{
			string filename = GetFileName(null,true);
			if(filename != null)
			{
				try
				{
					StreamWriter sw = new StreamWriter(filename);
					String text = richTextBox1.Text;
					if( eolMenuItem.Checked )
					{ // if we're on windows
						text = text.Replace("\r\n", "\n");
						text = text.Replace("\n","\r\n");
					}
					sw.Write( text );
					sw.Close();
				}
				catch
				{
					MessageBox.Show("ERROR! Could not save to file {0}",filename);
				}
			}
		}
		public static string aboutMsg= string.Format(
@"{0}
NEW!!! ->  Double click on a player to bring up the new player editing GUI with that player selected.

====================BASIC USAGE=================
1. Load TSB nes or snes rom.
2. View Contents.
3. Modify player attributes or schedules.
4. Apply to Rom.
=============================================
This tool was created to make it easier and faster to edit players and schedules in 
Tecmo Super Bowl (nes version, 32 team nes version, snes TSB1 version). 
It is intended to be used as a complement to TSBM (from emuware).It does not do 
everything that TSBM or TSBM 2000 does. It's purpose is to make it easy and fast to 
modify player names, player attributes, team attributes and season schedules.

This program can read from standard in or from a file (when executed from command line)
View the README to learn how to use it from the command line. To view command line options
type 'TSBToolSupreme /?' at the command prompt.

Use this program at your own risk. TSBToolSupreme creator is not responsible for anyting bad that happens.
User takes full responsibility for anything that happens as a result of usung this program.
Do not Break copyright laws.

This Program is not endorsed or related to the Tecmo video game company.
",MainClass.version);

		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(MainGUI.aboutMsg);
		}
		#region Searching Functionality
		/// <summary>
		/// 
		/// </summary>
		private string searchString = "";

		private bool SetSearchString()
		{
			bool ret = false;
			string result = StringInputDlg.GetString(
                                           "Enter Search String",
                                           "Please enter text (or a regex) to search for.",
				                           searchString);

			if(!result.Equals(""))
			{
				searchString = result;
				ret= true;
			}
			return ret;
		}

		private bool FindNextMatch()
		{
			bool ret = false;
			bool wrapped =false;
			string message = "NotFound";

			if( searchString != null && !searchString.Equals("") )
			{
				Regex r;
				r = new Regex(searchString,RegexOptions.IgnoreCase);
				Match m = r.Match(richTextBox1.Text, richTextBox1.SelectionStart);

				if(m.Length == 0)
				{ // continue at the top if not found
					m = r.Match(richTextBox1.Text);
					wrapped = true;
				}
				if(m.Length > 0)
				{
					richTextBox1.SelectionStart = m.Index+m.Length;
					ret=true;
					if(!wrapped)
						message = "Found";
					else
						message = "Text found, search starting at beginning.";
				}
			}
			statusBar1.Text = message;
			return ret;
		}

		private bool FindPrevMatch()
		{
			bool ret = false;
			string message = "Not Found";

			if( searchString!= null && !searchString.Equals("") )
			{
				Regex r = new Regex(searchString,RegexOptions.IgnoreCase);
				MatchCollection mc = r.Matches(richTextBox1.Text);
				Match m = null;
				if(mc.Count > 0)
				{
					m = mc[mc.Count-1];
				}
				else 
				{
					ret = false;
					goto end;
				}
				int i =0;
				while(mc[i].Index < richTextBox1.SelectionStart-mc[i].Length)
					m=mc[i++];
				if(m != null && m.Length != 0)
				{
					richTextBox1.SelectionStart = m.Index+m.Length;
					message= "Found";
					ret = true;
				}
			}
			end:
			statusBar1.Text = message;
			return ret;
		}

		#endregion

		private void richTextBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if( e.Control )
			{
				if(e.KeyCode == Keys.F)
				{
					if(SetSearchString())
						FindNextMatch();
				}
				else if(e.KeyCode == Keys.F3)
					FindPrevMatch();
				else if( e.KeyCode == Keys.G )
					EditPlayer();
				else if( e.KeyCode == Keys.L )
				{
					CutLine();
				}
				else if( e.KeyCode == Keys.V )
				{
					richTextBox1.Paste(DataFormats.GetFormat(DataFormats.Text));
					e.Handled = true;
				}
			}
			else if( e.Shift )
			{
				if(e.KeyCode == Keys.F3)
					FindPrevMatch();
			}
			else if(e.KeyCode == Keys.F3)
				FindNextMatch();
			else if( e.KeyCode == Keys.F2)
				FindPrevMatch();
			
		}

		private void findMenuItem_Click(object sender, System.EventArgs e)
		{
			if(SetSearchString())
				FindNextMatch();
		}

		private void findNextMenuItem_Click(object sender, System.EventArgs e)
		{
			FindNextMatch();
		}

		private void findPrevMenuItem_Click(object sender, System.EventArgs e)
		{
			FindPrevMatch();
		}

		private string ExecTsbSeasonGen( string argument)
		{
			string stdout = null;
			string stderr = null;
			string ret = null;
			
			if( File.Exists( argument ) )
				argument = "-config:"+argument;

			cleanupProcess();
			process = new Process();
			process.StartInfo.UseShellExecute        = false;
			
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError  = true;
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.FileName               = programExecName;
			process.StartInfo.Arguments              = argument;
			process.StartInfo.WorkingDirectory       = ".";
			//process = Process.Start(programExecName, argument );
			
			//process.WaitForExit();
			try
			{
				process.Start();
				stdout = process.StandardOutput.ReadToEnd();
			}
			catch{}
			try
			{
				stderr = process.StandardError.ReadToEnd();
			}
			catch{}

			if( stdout != null && stdout != "")
			{
				ret = stdout;
				if( stderr != null && stderr.IndexOf("Error") > -1 || stderr.IndexOf("Warning") > -1  )
					MessageBox.Show(stderr);
			}
			else if(stderr != null && stderr != "")
				ret = stderr;
			else
				ret = null;

			return ret;
		}

		private string CallTsbSeasonGen(string arguments)
		{
			try
			{
				Assembly ass = Assembly.LoadFrom(programExecName);
				
				//Assembly.Load("TSBSeasonGen");
			}
			catch(Exception e) {
				MessageBox.Show(e.Message );
			}
			return "";
		}

		private void tsbSeasonGenMenuItem_Click(object sender, EventArgs e)
		{
			string args = StringInputDlg.GetString("Enter arguments for TSBSeasonGen",
                                                   "Enter a year or a config file\n" );

			if( args != null && args != "" )
			{
				string output = ExecTsbSeasonGen(args );
				//string output = CallTsbSeasonGen(args);
				if( output != null )
				{
					SetText( output );
				}
			}
		}

		private void cleanupProcess()
		{
			if( process != null && !process.HasExited )
			{
				try
				{
					process.Kill();
				}
				catch{}
			}
		}

		private void DeleteTrailingCommas()
		{
			string txt = InputParser.DeleteTrailingCommas( richTextBox1.Text );
			SetText( txt);
		}
		
		private string GetTeam(int textPosition)
		{
			string team = "bills";
			Regex r = new Regex("TEAM\\s*=\\s*([a-zA-Z49]+)");
			MatchCollection mc = r.Matches(richTextBox1.Text);
			Match theMatch = null;

			foreach(Match m in mc)
			{
				if(m.Index > textPosition )
					break;
				theMatch = m;
			}

			if( theMatch != null )
			{
				team = theMatch.Groups[1].Value;
			}
			return team;
		}

		private void ModifyTeams()
		{
			string team = GetTeam(richTextBox1.SelectionStart);
			ModifyTeams(team);
		}

		private void ModifyTeams(string team)
		{
			ModifyTeamForm form = new ModifyTeamForm();
			form.Data = richTextBox1.Text;
			form.CurrentTeam = team;

			if( form.ShowDialog(this) == DialogResult.OK )
			{
				int index = richTextBox1.SelectionStart;
				SetText( form.Data);
				if( richTextBox1.Text.Length > index)
				{
					richTextBox1.SelectionStart = index;
					richTextBox1.ScrollToCaret();
				}
			}
			form.Dispose();
		}

		private void ModifyColors()
		{
			string team = GetTeam(richTextBox1.SelectionStart);
			ModifyColors(team);
		}

		private void ModifyColors(string team)
		{
			UniformEditForm form = new UniformEditForm();
			form.Data = richTextBox1.Text;
			form.CurrentTeam = team;

			if( form.ShowDialog(this) == DialogResult.OK )
			{
				int index = richTextBox1.SelectionStart;
				SetText( form.Data);
				if( richTextBox1.Text.Length > index)
				{
					richTextBox1.SelectionStart = index;
					richTextBox1.ScrollToCaret();
				}
			}
			form.Dispose();
		}

		private void ModifyPlayers(string team, string position)
		{
			AttributeForm form   = new AttributeForm();
			form.Data            = richTextBox1.Text;
			form.CurrentTeam     = team;
			form.CurrentPosition = position;
			form.AutoUpdatePlayersUI = true;

			if( form.ShowDialog(this) == DialogResult.OK )
			{
				//int spot = VScrollPos;
				int spot2 = richTextBox1.SelectionStart;
				SetText( form.Data );
				if( richTextBox1.Text.Length > spot2 )
				{
					richTextBox1.SelectionStart = spot2;
				}
			}
		}

		private void MainGUI_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			cleanupProcess();
		}

		private void offensivePrefMenuItem_Click(object sender, System.EventArgs e)
		{
			this.offensivePrefMenuItem.Checked = !this.offensivePrefMenuItem.Checked;
		}

		private void testScheduleMenuItem_Click(object sender, System.EventArgs e)
		{
			if( tool.OutputRom != null )
			{
				string sch = tool.GetSchedule();
				SetText( sch );
			}
			else
				MessageBox.Show("Load rom first!.");
		}

		private void eolMenuItem_Click(object sender, System.EventArgs e)
		{
			eolMenuItem.Checked = !eolMenuItem.Checked;
		}

		private void tsbSeasonGen_optionsMenuItem_Click(object sender, EventArgs e)
		{
			if( File.Exists( seasonGenOptionFile) && Path.DirectorySeparatorChar == '\\' )
			{
				Process process = new Process();
				process.StartInfo.UseShellExecute        = false;
			
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError  = true;
				process.StartInfo.CreateNoWindow         = true;
				process.StartInfo.FileName               = "Notepad.exe";
				process.StartInfo.Arguments              = seasonGenOptionFile;
				process.StartInfo.WorkingDirectory       = ".";
				//process = Process.Start(programExecName, argument );
			
				//process.WaitForExit();
				try
				{
					process.Start();
				}
				catch{}
			}
			else
			{
				MessageBox.Show("Couldn't find Season gen options file.");
			}
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			string text  = "";
			string text2 = "";
			int textStart  = 0;
			int textLength = 0;
			bool splice = false;

			if( richTextBox1.SelectionLength > 0 )
			{
				text = richTextBox1.SelectedText;
				textStart = richTextBox1.SelectionStart;
				textLength = richTextBox1.SelectionLength;
				splice = true;
			}
			else if( richTextBox1.Text.Length > 0)
				text = richTextBox1.Text;

			NumberForm nf = new NumberForm(text);
			
			if( nf.ShowDialog() == DialogResult.OK )
			{
				text2 = nf.GetResult();
				if( text2 != null && text2 != string.Empty )
				{
					if( splice )
					{
						if( text2.EndsWith("\n") && richTextBox1.Text[textStart] == '\n' )
							text2 = text2.Substring(0, text2.Length -1 );

						string tmp = richTextBox1.Text.Substring(0, textStart);
						tmp += text2;
						tmp += richTextBox1.Text.Substring(textStart + textLength );
						SetText( tmp );
					}
					else
						SetText( text2 );
				}
			}
			nf.Dispose();
		}

		private void EditPlayer()
		{
			int pos       = richTextBox1.SelectionStart;
			int lineStart = 0;
			int posLen    = 0;
			string position = "QB1";
			string team   = "bills";

			if( pos > 0 && pos < richTextBox1.Text.Length )
			{
				int i =0;
				for(i = pos; i > 0; i-- )
				{
					if(richTextBox1.Text[i] == '\n')
					{
						lineStart = i+1;
						break;
					}
				}
				i = lineStart;
				char current =richTextBox1.Text[i];
				while( i < richTextBox1.Text.Length && current != ' ' && 
					current != ',' && current != '\n')
				{
					posLen++;
					i++;
					current = richTextBox1.Text[i];
				}
				position = richTextBox1.Text.Substring(lineStart, posLen);

				team = GetTeam(pos);
				ModifyPlayers(team, position);
			}
		}

		private void EditPlayers_Click(object sender, System.EventArgs e)
		{
			EditPlayer();
		}

		private void loadTSBButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Control && e.KeyCode == Keys.G )
				EditPlayer();
		}

		private void mOffensiveFormationsMenuItem_Click(object sender, System.EventArgs e)
		{
			mOffensiveFormationsMenuItem.Checked = !mOffensiveFormationsMenuItem.Checked;
		}

		private void mPlaybookMenuItem_Click(object sender, System.EventArgs e)
		{
			mPlaybookMenuItem.Checked = !mPlaybookMenuItem.Checked;
		}

		private void mCutMenuItem_Click(object sender, System.EventArgs e)
		{
			richTextBox1.Cut();
		}

		private void mCopyMenuItem_Click(object sender, System.EventArgs e)
		{
			richTextBox1.Copy();
		}

		private void mPasteMenuItem_Click(object sender, System.EventArgs e)
		{
			 richTextBox1.Paste(DataFormats.GetFormat(DataFormats.Text));
			//richTextBox1.Paste();
		}

		private void mDeleteCommasMenuItem_Click(object sender, System.EventArgs e)
		{
			DeleteTrailingCommas();
		}

		private void mSelectAllMenuItem_Click(object sender, System.EventArgs e)
		{
			richTextBox1.SelectAll();
		}

		private void mEditTeamsMenuItem_Click(object sender, System.EventArgs e)
		{
			ModifyTeams();
		}

		private void DoubleClicked()
		{
			string line =  GetLine (richTextBox1.SelectionStart);
			if( line == null )
				return;
			if( line.IndexOf("TEAM") > -1 || line.IndexOf("PLAYBOOK") > -1 )
			{
				ModifyTeams();
			}
			else if( line.IndexOf("COLORS") > -1 )
			{
				ModifyColors();
			}
			else
				EditPlayer();
		}

		/// <summary>
		/// Cuts the current line of text.
		/// </summary>
		private void CutLine()
		{
			int ls = GetLineStart();
			int le = GetLineEnd();
			int length = le - ls+1;
			if( length > -1 )
			{
				richTextBox1.SelectionStart = ls;
				richTextBox1.SelectionLength = length;
				richTextBox1.Cut();
			}
		}

		/// <summary>
		/// returns the line that linePosition falls on
		/// </summary>
		/// <param name="textPosition"></param>
		/// <returns></returns>
		private string GetLine(int textPosition)
		{
			string ret = null;
			if( textPosition < richTextBox1.Text.Length )
			{
				int i=0;
				int lineStart = 0;
				int posLen = 0;
				for(i = textPosition; i > 0; i-- )
				{
					if(richTextBox1.Text[i] == '\n')
					{
						lineStart = i+1;
						break;
					}
				}
				i = lineStart;
				if( i < richTextBox1.Text.Length )
				{
					char current =richTextBox1.Text[i];
					while( i < richTextBox1.Text.Length-1 /*&& current != ' ' && 
					current != ',' */ && current != '\n')
					{
						posLen++;
						i++;
						current = richTextBox1.Text[i];
					}
					ret = richTextBox1.Text.Substring(lineStart, posLen);
				}
			}
			return ret;
		}

		/// <summary>
		/// returns the position of the start of the current line.
		/// </summary>
		/// <returns></returns>
		private int GetLineStart()
		{
			int i=0;
			int textPosition = richTextBox1.SelectionStart;
			if( textPosition >= richTextBox1.Text.Length)
			{
				textPosition--;
			}
			int lineStart = 0;
			for(i = textPosition; i > 0; i-- )
			{
				if( richTextBox1.Text[i] == '\n')
				{
					lineStart = i+1;
					break;
				}
			}
			return lineStart;
		}

		/// <summary>
		/// returns the position of the end of the current line.
		/// </summary>
		/// <returns></returns>
		private int GetLineEnd()
		{
//			int ret = 0;
			int i = richTextBox1.SelectionStart;
			if( i >= richTextBox1.Text.Length )
			{
				return richTextBox1.Text.Length-1; 
			}
			char current =richTextBox1.Text[i];
			while( i < richTextBox1.Text.Length /*&& current != ' ' && 
					current != ',' */ && current != '\n')
			{
//				ret++;
				i++;
				current = richTextBox1.Text[i];
			}
			return i;
		}

		private static DateTime m_LastTime;

		private void richTextBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DateTime now = DateTime.Now;
			if( m_LastTime.Day    == now.Day && 
				m_LastTime.Hour   == now.Hour &&
				m_LastTime.Second == now.Second)
			{
				DoubleClicked();
			}
			m_LastTime = now;
		}

		private void mChangeFontItem_Click(object sender, System.EventArgs e)
		{
			FontDialog dlg = new FontDialog();
			dlg.Font = this.Font;
			if( dlg.ShowDialog() == DialogResult.OK )
			{
				this.Font = dlg.Font;
				Font tbFont = new Font(richTextBox1.Font.FontFamily, dlg.Font.Size);
				richTextBox1.Font = tbFont;
				this.ApplyAutoScaling();
			}

		}

		private void mTestMenuItem_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			UniformEditForm form = new UniformEditForm();
			form.HomePantsColorString= "3C";
			form.ShowDialog(this);
			this.richTextBox1.Text = form.Result;
			form.Dispose();
		}

		private void mColorsMenuItem_Click(object sender, System.EventArgs e)
		{
			mColorsMenuItem.Checked = !mColorsMenuItem.Checked;
			TecmoTool.ShowColors = mColorsMenuItem.Checked;
		}

		private void mEditColorsMenuItem_Click(object sender, System.EventArgs e)
		{
			ModifyColors();
		}

		private void mGetLocationsMenuItem_Click(object sender, System.EventArgs e)
		{
			if( tool != null && tool.OutputRom != null )
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.RestoreDirectory=true;

				if( dlg.ShowDialog(this) == DialogResult.OK )
				{
					string result = MainClass.GetLocations(dlg.FileName, tool.OutputRom);
					RichTextDisplay disp = new RichTextDisplay();
					disp.ContentBox.Font = richTextBox1.Font;
					disp.ContentBox.Text = result;
					disp.Text = string.Concat("Results from '", dlg.FileName, "'");
					disp.Show();
				}
				dlg.Dispose();
			}
			else
			{
				MessageBox.Show(this, "Please load a ROM first", "Error!!",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

	}
}
