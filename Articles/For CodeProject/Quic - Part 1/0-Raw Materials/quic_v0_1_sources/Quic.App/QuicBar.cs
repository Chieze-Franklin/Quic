using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quic;
using Quic.Messaging;

namespace Quic.App
{
    public partial class QuicBar : Form
    {
        bool max, showNotif;
        Point restoreLocation = new Point();
        Size restoreSize = new Size();
        int minHeight = 24;
        TitleBar titleBar;
        NotificationPanel notifPanel;

        public QuicBar()
        {
            restoreLocation = this.Location;
            restoreSize = this.Size = this.MinimumSize = new Size(500, minHeight);

            this.BackColor = Color.Pink;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TransparencyKey = this.BackColor;
            maximizedChanged = new EventHandler(DefaultEventHandler);
            this.Maximized = true;
            this.Icon = Properties.Resources.Icon;

            //titleBar
            titleBar = new TitleBar(this);
            this.Controls.Add(titleBar);
            this.RegisterToMoveWindow(titleBar);

            //notifPanel
            notifPanel = new NotificationPanel()
            {
                Dock = DockStyle.Bottom,
                Location = new Point(titleBar.Location.X, titleBar.Location.Y + titleBar.Height + 1)
            };

            this.Shown += delegate { this.TopMost = true; };

            Messenger.Prompted += Messenger_Prompted;
        }

        void Messenger_Prompted(object sender, Messenger.MessengerEventArgs e)
        {
            MessageBox.Show(e.Message, "Quic");
        }

        Point initialPoint, finalPoint;
        public void RegisterToMoveWindow(Control control)
        {
            control.DoubleClick += delegate { this.Maximized = !this.Maximized; };
            control.MouseDown += delegate(object sender, MouseEventArgs e)
            {
                initialPoint = e.Location;
            };
            control.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (!Maximized)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        finalPoint = e.Location;

                        int deltaX = (finalPoint.X - initialPoint.X);
                        int deltaY = (finalPoint.Y - initialPoint.Y);
                        this.Location = new Point(this.Location.X + deltaX, this.Location.Y + deltaY);
                    }
                }
            };
        }

        void FitToScreen()
        {
            restoreLocation = this.Location;
            restoreSize = this.Size;
            this.Location = new Point(0, 0);
            Rectangle rect = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            rect = Rectangle.Union(rect, Screen.PrimaryScreen.WorkingArea);
            this.Width = rect.Width;
            if (!ShowNotifications)
                this.Height = minHeight;
            //this.Size = new System.Drawing.Size(rect.Width, this.Height);//24);
            //
        }
        void UnfitFromScreen()
        {
            this.Location = restoreLocation;
            this.Size = restoreSize;
            //
        }
        /// <summary>
        /// Gets or sets a value to determine if the window is maximized
        /// </summary>
        public bool Maximized
        {
            get { return max; }
            set
            {
                if (!this.IsFixed && max != value)
                {
                    if (value)
                        FitToScreen();
                    else
                        UnfitFromScreen();

                    max = value;
                    maximizedChanged(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// Gets or sets a value to determine if the window is fixed or can be resized
        /// </summary>
        public bool IsFixed { get; set; }

        public bool ShowNotifications 
        {
            get { return showNotif; }
            set 
            {
                if (showNotif != value) 
                {
                    if (value)
                    {
                        restoreSize = new Size(restoreSize.Width, minHeight + (notifPanel.Height + 2));
                        this.Height = minHeight + notifPanel.Height + 2;
                        notifPanel.RefreshNotificationList();
                        this.Controls.Add(notifPanel);
                    }
                    else
                    {
                        restoreSize = new Size(restoreSize.Width, minHeight);
                        this.Height = minHeight;
                        this.Controls.Remove(notifPanel);
                    }

                    showNotif = value;
                }
            }
        }

        event EventHandler maximizedChanged;
        void DefaultEventHandler(object sender, EventArgs e) { }
        public event EventHandler MaximizedChanged
        {
            add
            {
                if (maximizedChanged == null)
                    maximizedChanged = new EventHandler(DefaultEventHandler);
                maximizedChanged += value;
            }
            remove
            {
                if (maximizedChanged == null)
                    maximizedChanged = new EventHandler(DefaultEventHandler);
                maximizedChanged -= value;
            }
        }
    }

    public class TitleBar : Panel
    {
        QuicBar owner;
        Label btnIconBox, lblAppName, btnNotify, btnMinimize, btnMaximize, btnClose;
        Panel dynamicPanel;
        FileBar fbSourceFile; 
        DirectoryBar fbOutputDir;
        int offset = 1, smSz = 22, sz = 24, smFb = 350, fb = 450;
        bool showOutBar;

        public TitleBar(QuicBar owner)
        {
            this.owner = owner;

            ToolTip tooltip = new ToolTip();

            this.BackColor = Color.Black;
            this.Dock = DockStyle.Top;
            this.ForeColor = Color.White;
            this.Height = sz;

            //fbSourceFile
            fbSourceFile = new FileBar()
            {
                Size = new Size(fb, smSz),
            };
            owner.RegisterToMoveWindow(fbSourceFile);
            fbSourceFile.Run += fbSourceFile_Run;

            //fbOutputDir
            fbOutputDir = new DirectoryBar()
            {
                Size = new Size(fb, smSz),
            };
            owner.RegisterToMoveWindow(fbOutputDir);
            fbOutputDir.Run += fbOutputDir_Run;

            NotificationImageNormal = Properties.Resources.NotifNormal;
            NotificationImageFocus = Properties.Resources.NotifNormal2;
            //btnNotify
            btnNotify = new Label()
            {
                BackgroundImage = NotificationImageNormal,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(smSz, smSz)
            };
            btnNotify.MouseHover += delegate { tooltip.SetToolTip(btnNotify, "Show/hide notifications"); };
            btnNotify.MouseEnter += delegate { btnNotify.BackgroundImage = NotificationImageFocus; };
            btnNotify.MouseLeave += delegate { btnNotify.BackgroundImage = NotificationImageNormal; };
            btnNotify.Click += delegate { owner.ShowNotifications = !owner.ShowNotifications; };

            //btnMinimize
            btnMinimize = new Label()
            {
                BackgroundImage = Properties.Resources.Minimize,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(smSz, smSz)
            };
            btnMinimize.MouseEnter += delegate { btnMinimize.BackgroundImage = Properties.Resources.Minimize2; };
            btnMinimize.MouseLeave += delegate { btnMinimize.BackgroundImage = Properties.Resources.Minimize; };
            btnMinimize.Click += delegate { owner.WindowState = FormWindowState.Minimized; };


            if (owner.Maximized)
            {
                MaximizeImageNormal = Properties.Resources.Restore;
                MaximizeImageFocus = Properties.Resources.Restore2;
            }
            else
            {
                MaximizeImageNormal = Properties.Resources.Maximize;
                MaximizeImageFocus = Properties.Resources.Maximize2;
            }
            //btnMaximize
            btnMaximize = new Label()
            {
                BackgroundImage = MaximizeImageNormal,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(smSz, smSz)
            };
            btnMaximize.MouseEnter += delegate { btnMaximize.BackgroundImage = MaximizeImageFocus; };
            btnMaximize.MouseLeave += delegate { btnMaximize.BackgroundImage = MaximizeImageNormal; };
            btnMaximize.Click += delegate { owner.Maximized = !owner.Maximized; };

            //btnClose
            btnClose = new Label()
            {
                BackgroundImage = Properties.Resources.Close,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(smSz, smSz)
            };
            btnClose.MouseEnter += delegate { btnClose.BackgroundImage = Properties.Resources.Close2; };
            btnClose.MouseLeave += delegate { btnClose.BackgroundImage = Properties.Resources.Close; };
            btnClose.Click += delegate { owner.Close(); };

            //dynamicPanel
            dynamicPanel = new Panel()
            {
                Height = sz,
                Width = 0
            };
            this.Controls.Add(dynamicPanel);

            //btnIconBox
            btnIconBox = new Label()
            {
                BackgroundImage = Properties.Resources.IconImage,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(offset, offset),
                Size = new Size(smSz, smSz)
            };
            owner.RegisterToMoveWindow(btnIconBox);
            this.Controls.Add(btnIconBox);

            //lblAppName
            lblAppName = new Label()
            {
                AutoEllipsis = true,
                Font = new System.Drawing.Font(this.Font.FontFamily, 16.0F),
                ForeColor = Color.White,
                Size = new Size(70, smSz),
                Text = "QUIC",
                TextAlign = ContentAlignment.BottomLeft
            };
            lblAppName.Location = new Point(btnIconBox.Location.X + btnIconBox.Width + offset, offset);
            this.Controls.Add(lblAppName);
            owner.RegisterToMoveWindow(lblAppName);
            //owner.SizeChanged += delegate { lblAppName.Width = owner.Width / 5; };

            owner.MaximizedChanged += owner_MaximizedChanged;
            
            this.SizeChanged += TitleBar_SizeChanged;

            //I used the variables instead of the properties so that RefreshDynamicPanel() is not called unnecessarily
            showOutBar = true;
            RefreshDynamicPanel();

            Messenger.Notified += Messenger_Notified;
            Messenger.NotificationsCleared += Messenger_NotificationsCleared;
        }

        void fbSourceFile_Run(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fbSourceFile.FilePath) && !string.IsNullOrWhiteSpace(fbOutputDir.FilePath))
                {
                    Messenger.ClearNotifications();
                    QuicDocument doc = QuicDocument.Load(fbSourceFile.FilePath);
                    doc.Render(fbOutputDir.FilePath);

                    //a success notif
                    if (Messenger.Notifications.Length == 0) 
                    {
                        Messenger.Notify(
                       new QuicException("Build complete.", fbSourceFile.FilePath) { MessageType = MessageType.Success });
                    }
                }
            }
            catch (QuicException ex)
            {
                Messenger.Notify(ex);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Quic");
                Messenger.Prompt(ex.Message, MessageType.Error);
            }
        }

        void fbOutputDir_Run(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fbOutputDir.FilePath))
                {
                    if (Directory.Exists(fbOutputDir.FilePath))
                        Process.Start(fbOutputDir.FilePath);
                }
            }
            catch (QuicException ex)
            {
                Messenger.Notify(ex);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Quic");
                Messenger.Prompt(ex.Message, MessageType.Error);
            }
        }

        void owner_MaximizedChanged(object sender, EventArgs e)
        {
            if (owner.Maximized)
            {
                MaximizeImageNormal = Properties.Resources.Restore;
                MaximizeImageFocus = Properties.Resources.Restore2;
                lblAppName.Visible = true;
                fbSourceFile.Width = fbOutputDir.Width = fb;
                ShowOutputFileBar = true;
            }
            else
            {
                MaximizeImageNormal = Properties.Resources.Maximize;
                MaximizeImageFocus = Properties.Resources.Maximize2;
                lblAppName.Visible = false;
                fbSourceFile.Width = smFb;
                ShowOutputFileBar = false;
            }
        }

        void TitleBar_SizeChanged(object sender, EventArgs e)
        {
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
        }

        void Messenger_Notified(object sender, Messenger.NotifyEventArgs e)
        {
            if (e.Notification != null)
            {
                if (e.Notification.MessageType == MessageType.Error)
                {
                    NotificationImageNormal = Properties.Resources.NotifError;
                    NotificationImageFocus = Properties.Resources.NotifError2;
                }
                else if (e.Notification.MessageType == MessageType.Info)
                {
                    NotificationImageNormal = Properties.Resources.NotifInfo;
                    NotificationImageFocus = Properties.Resources.NotifInfo2;
                }
                else if (e.Notification.MessageType == MessageType.Question)
                {
                    NotificationImageNormal = Properties.Resources.NotifQuestion;
                    NotificationImageFocus = Properties.Resources.NotifQuestion2;
                }
                else if (e.Notification.MessageType == MessageType.Success)
                {
                    NotificationImageNormal = Properties.Resources.NotifSuccess;
                    NotificationImageFocus = Properties.Resources.NotifSuccess2;
                }
                else if (e.Notification.MessageType == MessageType.Warning)
                {
                    NotificationImageNormal = Properties.Resources.NotifWarning;
                    NotificationImageFocus = Properties.Resources.NotifWarning2;
                }
                else
                {
                    NotificationImageNormal = Properties.Resources.NotifNormal;
                    NotificationImageFocus = Properties.Resources.NotifNormal2;
                }
            }

            btnNotify.BackgroundImage = NotificationImageNormal;
        }
        void Messenger_NotificationsCleared(object sender, EventArgs e)
        {
            NotificationImageNormal = Properties.Resources.NotifNormal;
            NotificationImageFocus = Properties.Resources.NotifNormal2;

            btnNotify.BackgroundImage = NotificationImageNormal;
        }

        void RefreshDynamicPanel()
        {
            dynamicPanel.Controls.Clear();

            int currentX = offset;
            int increment = 0;
            dynamicPanel.Width = 0;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);

            //add fbSourceFile
            increment = (offset + fbSourceFile.Width);
            fbSourceFile.Location = new Point(currentX, offset);
            dynamicPanel.Width += increment;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
            currentX += increment;
            dynamicPanel.Controls.Add(fbSourceFile);

            ////add fbOutputDir
            if (ShowOutputFileBar)
            {
                increment = (offset + fbOutputDir.Width);
                fbOutputDir.Location = new Point(currentX, offset);
                dynamicPanel.Width += increment;
                dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
                currentX += increment;
                dynamicPanel.Controls.Add(fbOutputDir);
            }

            //add notify
            increment = (offset + btnNotify.Width);
            btnNotify.Location = new Point(currentX, offset);
            dynamicPanel.Width += increment;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
            currentX += increment;
            dynamicPanel.Controls.Add(btnNotify);

            //add minimize
            increment = (offset + btnMinimize.Width);
            btnMinimize.Location = new Point(currentX, offset);
            dynamicPanel.Width += increment;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
            currentX += increment;
            dynamicPanel.Controls.Add(btnMinimize);

            //add maximize
            increment = (offset + btnMaximize.Width);
            btnMaximize.Location = new Point(currentX, offset);
            dynamicPanel.Width += increment;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
            currentX += increment;
            dynamicPanel.Controls.Add(btnMaximize);

            //add close
            increment = (offset + btnClose.Width);
            btnClose.Location = new Point(currentX, offset);
            dynamicPanel.Width += increment;
            dynamicPanel.Location = new Point(this.Width - (dynamicPanel.Width + offset), 0);
            currentX += increment;
            dynamicPanel.Controls.Add(btnClose);
        }

        /// <summary>
        /// Gets or sets a value to determine if the maximize/restore button is shown
        /// </summary>
        public bool ShowOutputFileBar
        {
            get { return showOutBar; }
            set
            {
                if (showOutBar != value)
                {
                    showOutBar = value;
                    RefreshDynamicPanel();
                }
            }
        }

        Image MaximizeImageNormal { get; set; }
        Image MaximizeImageFocus { get; set; }
        Image NotificationImageNormal { get; set; }
        Image NotificationImageFocus { get; set; }
    }

    public class FileBar : Panel
    {
        protected Label btnAction, btnBrowse;
        protected TextBox txtFile;
        int offset = 1, smSz = 19;
        OpenFileDialog openSourceFileDialog;
        protected ToolTip tooltip;

        public FileBar()
        {
            //run
            run = new EventHandler(DefaultEventHandler);

            //openSourceFileDialog
            openSourceFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "All|*.*",
                Multiselect = false
            };

            //tooltip
            tooltip = new ToolTip();

            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            //btnAction
            btnAction = new Label()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                BackgroundImage = Properties.Resources.Play,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(offset, offset),
                Size = new Size(smSz, smSz)
            };
            this.Controls.Add(btnAction);
            btnAction.MouseHover += delegate { OnActionMouseHover(); };
            btnAction.MouseEnter += delegate { btnAction.BackgroundImage = Properties.Resources.Play2; };
            btnAction.MouseLeave += delegate { btnAction.BackgroundImage = Properties.Resources.Play; };
            btnAction.Click += delegate { run(this, new EventArgs()); };

            //txtFile
            txtFile = new TextBox() 
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Location = new Point(btnAction.Location.X + btnAction.Width + offset, offset),
                Width = this.Width - ((smSz + offset) * 2),
            };
            this.Controls.Add(txtFile);
            txtFile.TextChanged += delegate { FilePath = txtFile.Text; };

            //btnBrowse
            btnBrowse = new Label()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackgroundImage = BrowseImageNormal,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(this.Width - (smSz + offset), offset),
                Size = new Size(smSz, smSz)
            };
            this.Controls.Add(btnBrowse);
            btnBrowse.MouseHover += delegate { OnBrowseMouseHover(); };
            btnBrowse.MouseEnter += delegate { btnBrowse.BackgroundImage = BrowseImageFocus; };
            btnBrowse.MouseLeave += delegate { btnBrowse.BackgroundImage = BrowseImageNormal; };
            btnBrowse.Click += delegate { OnBrowse(); };

            //this.SizeChanged += delegate 
            //{
            //    txtFile.Width = this.Width - (btnAction.Width + btnBrowse.Width);
            //};
        }

        protected virtual void OnActionMouseHover()
        {
            tooltip.SetToolTip(btnAction, "Build source file");
        }
        protected virtual void OnBrowseMouseHover()
        {
            tooltip.SetToolTip(btnBrowse, "Select source file");
        }
        protected virtual void OnBrowse()
        {
            if (openSourceFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFile.Text = openSourceFileDialog.FileName;
            }
        }

        public string FilePath { get; private set; }
        protected virtual Image BrowseImageNormal { get { return Properties.Resources.BrowseFile; } }
        protected virtual Image BrowseImageFocus { get { return Properties.Resources.BrowseFile2; } }

        event EventHandler run;
        void DefaultEventHandler(object sender, EventArgs e) { }
        public event EventHandler Run
        {
            add
            {
                if (run == null)
                    run = new EventHandler(DefaultEventHandler);
                run += value;
            }
            remove
            {
                if (run == null)
                    run = new EventHandler(DefaultEventHandler);
                run -= value;
            }
        }
    }

    public class DirectoryBar : FileBar 
    {
        FolderBrowserDialog openOutputDirDialog;

        public DirectoryBar()
        {
            //openOutputDirDialog
            openOutputDirDialog = new FolderBrowserDialog();
        }

        protected override void OnActionMouseHover()
        {
            tooltip.SetToolTip(btnAction, "Open output directory");
        }
        protected override void OnBrowseMouseHover()
        {
            tooltip.SetToolTip(btnBrowse, "Select output directory");
        }
        protected override void OnBrowse()
        {
            if (openOutputDirDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFile.Text = openOutputDirDialog.SelectedPath;
            }
        }

        protected override Image BrowseImageNormal { get { return Properties.Resources.BrowseFolder; } }
        protected override Image BrowseImageFocus { get { return Properties.Resources.BrowseFolder2; } }
    }

    public class NotificationPanel : Panel 
    {
        ListView listView;
        ToolStripContainer tscListViewCont;
        ToolStripTextBox searchBox;
        ToolStrip stripListViewToolBar;

        public NotificationPanel() 
        {
            BackColor = Color.Black;
            ForeColor = Color.White;
            Height = 200;

            //imgLst
            ImageList imgLst = new ImageList();
            imgLst.Images.Add("E", Properties.Resources.Error);
            imgLst.Images.Add("I", Properties.Resources.Info);
            imgLst.Images.Add("Q", Properties.Resources.Question);
            imgLst.Images.Add("S", Properties.Resources.Success);
            imgLst.Images.Add("W", Properties.Resources.Warning);

            //listView
            listView = new ListView();
            listView.AllowColumnReorder = false;
            listView.BackColor = Color.Black;
            listView.CheckBoxes = false;
            listView.Dock = DockStyle.Fill;
            listView.ForeColor = Color.White;
            listView.FullRowSelect = true;
            listView.LargeImageList = listView.SmallImageList = listView.StateImageList = imgLst;
            listView.MultiSelect = false;
            listView.ShowItemToolTips = true;
            listView.View = View.Details;
            listView.Columns.Add("", 20, HorizontalAlignment.Left); //icon
            listView.Columns.Add("", 20, HorizontalAlignment.Left); //S/N
            listView.Columns.Add("Message", 200, HorizontalAlignment.Left);
            listView.Columns.Add("File", 100, HorizontalAlignment.Left);
            listView.Columns.Add("Line", 50, HorizontalAlignment.Left);
            listView.Columns.Add("Column", 50, HorizontalAlignment.Left);

            //searchBox
            searchBox = new ToolStripTextBox() 
            {
                //BackColor = Color.Black,
                //ForeColor = Color.White,
                ToolTipText = "search"
            };
            searchBox.ToolTipText = "search";
            searchBox.KeyUp += searchBox_KeyUp;
            searchBox.TextChanged += searchBox_TextChanged;

            //stripListViewToolBar
            stripListViewToolBar = new ToolStrip();
            stripListViewToolBar.Dock = DockStyle.Fill;
            stripListViewToolBar.Items.Add(searchBox);

            //tscListView
            tscListViewCont = new ToolStripContainer();
            tscListViewCont.Dock = DockStyle.Fill;
            tscListViewCont.TopToolStripPanel.BackColor = tscListViewCont.ContentPanel.BackColor = Color.Black;
            tscListViewCont.TopToolStripPanel.ForeColor = tscListViewCont.ContentPanel.ForeColor = Color.White;
            tscListViewCont.TopToolStripPanel.Controls.Add(stripListViewToolBar);
            tscListViewCont.ContentPanel.Controls.Add(listView);

            //this
            this.Controls.Add(tscListViewCont);
            this.SizeChanged += NotificationPanel_SizeChanged;

            Messenger.Notified += Messenger_Notified;
            Messenger.NotificationsCleared += Messenger_NotificationsCleared;
        }

        void Messenger_Notified(object sender, Messenger.NotifyEventArgs e)
        {
            RefreshNotificationList();
        }

        void Messenger_NotificationsCleared(object sender, EventArgs e)
        {
            listView.Items.Clear();
        }
        void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HighlightNotifications(searchBox.Text);
            }
        }
        void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == string.Empty)
            {
                //was supposed to draw a search string and icon
            }
        }
        void NotificationPanel_SizeChanged(object sender, EventArgs e)
        {
            //listView
            listView.Columns[0].Width = (listView.Width * 5) / 100; //icon
            listView.Columns[1].Width = (listView.Width * 5) / 100; //S/N
            listView.Columns[2].Width = (listView.Width * 65) / 100;  //Message (used 65 instead of 70 to reduce the chances of seeing a horizontal scroll bar)
            listView.Columns[3].Width = (listView.Width * 10) / 100; //File
            listView.Columns[4].Width = (listView.Width * 5) / 100; //line
            listView.Columns[5].Width = (listView.Width * 5) / 100; //column
        }

        public void RefreshNotificationList() 
        {
            listView.Items.Clear();
            int index = 0;

            foreach (var notification in Messenger.Notifications) 
            {
                if (notification != null)
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = notification.MessageType.ToString()[0].ToString(); //the first char of the string representation of d enum

                    ListViewItem.ListViewSubItem indexItem = new ListViewItem.ListViewSubItem();
                    indexItem.Text = (++index).ToString();

                    ListViewItem.ListViewSubItem msgItem = new ListViewItem.ListViewSubItem();
                    if (notification.Message != null)
                        msgItem.Text = notification.Message;

                    ListViewItem.ListViewSubItem fileItem = new ListViewItem.ListViewSubItem();
                    if (notification.FilePath != null)
                        fileItem.Text = Path.GetFileName(notification.FilePath);

                    ListViewItem.ListViewSubItem lineItem = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem colItem = new ListViewItem.ListViewSubItem();
                    if (notification.HasLineInfo) 
                    {
                        lineItem.Text = notification.Line.ToString();
                        colItem.Text = notification.Column.ToString();
                    }

                    item.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { indexItem, msgItem, fileItem, lineItem, colItem });
                    listView.Items.Add(item);
                }
            }
        }
        void HighlightNotifications(string searchTerm)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.BackColor = Color.Black;
                if (searchTerm != null && searchTerm.Trim() != string.Empty)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        if (subItem.Text.ToLower().Contains(searchTerm.ToLower()))
                        {
                            item.BackColor = Color.Yellow;
                            break;
                        }
                    }
                }
            }
        }
    }
}
