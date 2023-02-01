using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralTool.General.WinForm
{
    /// <summary>
    /// 等待框帮助类
    /// </summary>
    public class WaitDialogHelper
    {
        #region Private 字段

        private DialogFrm dialog;

        private MaskPanel maskPanel;

        private Form parentFrm;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public WaitDialogHelper()
        {
            dialog = new DialogFrm();
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取或设置等待框上的文本信息
        /// </summary>
        public string Caption
        {
            get
            {
                if (IsClosed)
                {
                    return string.Empty;
                }
                else
                {
                    return dialog.Caption;
                }
            }

            set
            {
                if (!IsClosed)
                {
                    dialog.Caption = value;
                }
            }
        }

        /// <summary>
        /// 获取等待框是否已关闭
        /// </summary>
        public bool IsClosed { get; private set; }

        /// <summary>
        /// 获取或设置等待框上进度条进度
        /// </summary>
        public int ProgressValue
        {
            get
            {
                if (IsClosed)
                {
                    return 0;
                }
                else
                {
                    return dialog.ProgressValue;
                }
            }

            set
            {
                if (!IsClosed && value > -1)
                {
                    dialog.ProgressValue = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置等待框上进度条是否显示
        /// </summary>
        public bool ProgressVisible
        {
            get
            {
                if (IsClosed)
                {
                    return false;
                }
                else
                {
                    return dialog.ProgressVisible;
                }
            }

            set
            {
                if (!IsClosed)
                {
                    dialog.ProgressVisible = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置等待框的背景色
        /// </summary>
        public Color WaitPanelBackColor
        {
            get
            {
                if (IsClosed)
                {
                    return default;
                }
                else
                {
                    return dialog.BackColor;
                }
            }

            set
            {
                if (!IsClosed)
                {
                    dialog.BackColor = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置等待框的前景色
        /// </summary>
        public Color WaitPanelForeColor
        {
            get
            {
                if (IsClosed)
                {
                    return default;
                }
                else
                {
                    return dialog.ForeColor;
                }
            }

            set
            {
                if (!IsClosed)
                {
                    dialog.ForeColor = value;
                }
            }
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 关闭等待框
        /// </summary>
        public void CloseDialog()
        {
            EnableParentFrm(true);
            dialog.Close();
            dialog.Dispose();
            maskPanel.Visible = false;

            maskPanel.Dispose();
            parentFrm.LocationChanged -= ParentFrm_LocationChanged;
            parentFrm.SizeChanged -= ParentFrm_SizeChanged;
            IsClosed = true;
            dialog = null;
            maskPanel = null;
        }

        /// <summary>
        /// 显示等待框
        /// </summary>
        /// <param name="parentFrm">
        /// 要显示的等待框父窗体
        /// </param>
        /// <param name="caption">
        /// 在等待框上要显示的文本
        /// </param>
        /// <param name="alpha">
        /// 遮罩层的透明度
        /// </param>
        /// <param name="canDragAndResize">
        /// 主窗体是否可以响应拖动,更改大小事件(在对主窗体更改大小时视觉会有拖影)
        /// </param>
        public async void ShowDialog(Form parentFrm, string caption = "", int alpha = 220, bool canDragAndResize = false)
        {
            IsClosed = false;
            this.parentFrm = parentFrm;

            maskPanel = new MaskPanel(parentFrm, alpha);

            parentFrm.LocationChanged += ParentFrm_LocationChanged;
            parentFrm.SizeChanged += ParentFrm_SizeChanged;

            EnableParentFrm(false);

            maskPanel.Visible = true;

            //dialog.Show(parentFrm);
            //CenterToParent();

            dialog.Caption = caption;
            await Task.Run(() =>
             {
                 parentFrm.BeginInvoke(new Action(() =>
                 {
                     if (canDragAndResize)
                     {
                         dialog.Show(parentFrm);
                     }
                     else
                     {
                         dialog.ShowDialog(parentFrm);
                     }
                 }));
             });
            CenterToParent();
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 将等待框放置于父窗体中间
        /// </summary>
        private void CenterToParent()
        {
            dialog.Width = parentFrm.ClientSize.Width;
            dialog.Left = parentFrm.Left + (parentFrm.Width - dialog.Width) / 2;
            dialog.Top = parentFrm.Top + (parentFrm.Height - dialog.Height) / 2;

            dialog.Update();
            parentFrm.Update();
        }

        private void EnableParentFrm(bool enable)
        {
            foreach (Control control in parentFrm.Controls)
            {
                control.Enabled = enable;//避免用户可以通过TAB来切换到其它控件
            }

            parentFrm.UseWaitCursor = !enable;
        }

        private void ParentFrm_LocationChanged(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void ParentFrm_SizeChanged(object sender, EventArgs e)
        {
            CenterToParent();
        }

        #endregion Private 方法

        #region Private 类

        /// <summary>
        /// 等待框类
        /// </summary>
        private partial class DialogFrm : Form
        {
            #region Public 构造函数

            public DialogFrm()
            {
                InitializeComponent();
                DoubleBuffered = true;
            }

            #endregion Public 构造函数



            #region Public 属性

            public string Caption
            {
                get => captionLable.Text;

                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        Height = 45;
                        progressBar1.Height = 20;
                    }
                    else
                    {
                        if (ProgressVisible)
                        {
                            Height = 69;
                            progressBar1.Height = 20;
                            captionLable.Location = new Point(12, 44);
                            Validate();
                        }
                        else
                        {
                            Height = 38;
                            captionLable.Location = new Point(12, 12);
                        }
                    }
                    captionLable.Text = value;
                }
            }

            /// <summary>
            /// 进度值
            /// </summary>

            public int ProgressValue
            {
                get => progressBar1.Value;

                set
                {
                    if (value > 0)
                    {
                        progressBar1.Style = ProgressBarStyle.Continuous;
                    }
                    else
                    {
                        progressBar1.Style = ProgressBarStyle.Marquee;
                    }
                    progressBar1.Value = value;
                }
            }

            public bool ProgressVisible
            {
                get => progressBar1.Visible;

                set
                {
                    if (!value)
                    {
                        if (Height != 38)
                        {
                            Height = 38;
                            captionLable.Location = new Point(12, 12);
                        }
                    }
                    else
                    {
                        if (Height != 69)
                        {
                            Size = new Size(414, 69);
                            progressBar1.Height = 20;
                            progressBar1.Location = new Point(12, 12);
                            captionLable.Location = new Point(12, 44);
                        }
                    }
                    progressBar1.Visible = value;
                }
            }

            #endregion Public 属性
        }

        private partial class DialogFrm
        {
            #region Private 字段

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private readonly System.ComponentModel.IContainer components = null;

            private System.Windows.Forms.Label captionLable;

            private System.Windows.Forms.ProgressBar progressBar1;

            #endregion Private 字段



            #region Protected 方法

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">
            /// true if managed resources should be disposed; otherwise, false.
            /// </param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #endregion Protected 方法

            #region Private 方法

            /// <summary>
            /// Required method for Designer support - do not modify the contents of this method
            /// with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                progressBar1 = new System.Windows.Forms.ProgressBar();
                captionLable = new System.Windows.Forms.Label();
                SuspendLayout();

                // progressBar1
                progressBar1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
                progressBar1.Location = new System.Drawing.Point(12, 12);
                progressBar1.Name = "progressBar1";
                progressBar1.Size = new System.Drawing.Size(389, 20);
                progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                progressBar1.TabIndex = 0;
                progressBar1.Visible = true;

                // label1
                captionLable.AutoSize = true;
                captionLable.Location = new System.Drawing.Point(12, 44);
                captionLable.Name = "label1";
                captionLable.Size = new System.Drawing.Size(47, 12);
                captionLable.TabIndex = 1;
                captionLable.Text = "Caption";

                // DialogFrm
                AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                BackColor = System.Drawing.SystemColors.AppWorkspace;
                ClientSize = new System.Drawing.Size(414, 69);
                Controls.Add(captionLable);
                Controls.Add(progressBar1);
                ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                MaximizeBox = false;
                MinimizeBox = false;
                Name = "DialogFrm";
                ShowIcon = false;
                ShowInTaskbar = false;
                StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                Text = "DialogFrm";
                ResumeLayout(false);
                PerformLayout();
            }

            #endregion Private 方法
        }

        private class MaskPanel : Control
        {
            #region Private 字段

            private readonly System.ComponentModel.Container components = new System.ComponentModel.Container();

            private int _alpha = 125;

            private bool _isTransparent = true;

            #endregion Private 字段

            #region Public 构造函数

            //是否透明
            public MaskPanel(Control parent)
                : this(parent, 125)
            {
            }

            /// <summary>
            /// 初始化加载控件
            /// </summary>
            /// <param name="parent">
            /// 父控件
            /// </param>
            /// <param name="alpha">
            /// 透明度
            /// </param>
            public MaskPanel(Control parent, int alpha)
            {
                SetStyle(ControlStyles.Opaque, true);//设置背景透明
                base.CreateControl();
                _alpha = alpha;
                parent.Controls.Add(this);
                Parent = parent;
                Size = Parent.ClientSize;
                Dock = DockStyle.Fill;
                Left = 0;
                Top = 0;
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                BringToFront();

                Visible = false;
            }

            #endregion Public 构造函数



            #region Public 属性

            //设置透明度
            [Category("透明"), Description("设置透明度")]
            public int Alpha
            {
                get => _alpha;

                set
                {
                    _alpha = value;
                    Invalidate();
                }
            }

            [Category("透明"), Description("是否使用透明,默认为True")]
            public bool IsTransparent
            {
                get => _isTransparent;

                set
                {
                    _isTransparent = value;
                    Invalidate();
                }
            }

            #endregion Public 属性



            #region Protected 属性

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20; // 开启 WS_EX_TRANSPARENT,使控件支持透明
                    return cp;
                }
            }

            #endregion Protected 属性



            #region Protected 方法

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    components?.Dispose();
                }
                base.Dispose(disposing);
            }

            protected override void OnPaint(PaintEventArgs pe)
            {
                Pen labelBorderPen;
                SolidBrush labelBackColorBrush;
                if (_isTransparent)
                {
                    Color cl = Color.FromArgb(_alpha, BackColor);
                    labelBorderPen = new Pen(cl, 0);
                    labelBackColorBrush = new SolidBrush(cl);
                }
                else
                {
                    labelBorderPen = new Pen(BackColor, 0);
                    labelBackColorBrush = new SolidBrush(BackColor);
                }
                base.OnPaint(pe);
                pe.Graphics.DrawRectangle(labelBorderPen, 0, 0, Width, Height);
                pe.Graphics.FillRectangle(labelBackColorBrush, 0, 0, Width, Height);
            }

            #endregion Protected 方法
        }

        #endregion Private 类
    }
}