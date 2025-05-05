using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;

namespace Deskguise
{
    public partial class MainForm : Form
    {
        private Color NormalBackColor;
        private Color DraggingBackColor;

        private bool isDragging = false;
        private Point startDragPosition = Point.Empty;
        private FormWindowState lastWindowState = FormWindowState.Normal;

        private static string RegKey_WindowsColors = @"Control Panel\Colors";
        private static string RegKeySetting_DesktopBackground = "Background";
        private const int WM_SYSCOLORCHANGE = 0x0015;

        public MainForm()
        {
            InitializeComponent();
            UpdateToSystemBackgroundColor();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOLORCHANGE)
            {
                UpdateToSystemBackgroundColor();
            }
            base.WndProc(ref m);
        }

        private void UpdateToSystemBackgroundColor()
        {
            NormalBackColor = GetWindowsDesktopBackgroundColor();
            DraggingBackColor = ColorHelper.CreateSlightlyOffColor(NormalBackColor);
            if (this.WindowState == FormWindowState.Maximized)
            {
                BackColor = NormalBackColor;
            }
            else
            {
                BackColor = DraggingBackColor;
            }
        }


        private void MainForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BackColor = DraggingBackColor;
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != lastWindowState)
            {
                lastWindowState = WindowState;

                if (this.WindowState == FormWindowState.Maximized)
                {
                    BackColor = NormalBackColor;
                }
                else
                {
                    BackColor = DraggingBackColor;
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                return;
            }
            if (WindowState == FormWindowState.Maximized)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            isDragging = true;
            startDragPosition = e.Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && e.Button == MouseButtons.Left)
            {
                Point newLocation = e.Location;

                this.Location = new Point(
                    (this.Location.X - startDragPosition.X) + e.X,
                    (this.Location.Y - startDragPosition.Y) + e.Y
                );

                this.Update();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                startDragPosition = Point.Empty;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    this.Close();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
        }

        public static Color GetWindowsDesktopBackgroundColor()
        {
            return GetWindowsSystemColor(RegKeySetting_DesktopBackground);
        }

        public static Color GetWindowsSystemColor(string uiElement)
        {
            string color = null;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey_WindowsColors, false))
            {
                if (key != null)
                {
                    color = (string)key.GetValue(uiElement);
                }
            }

            if (color != null)
            {
                return ColorHelper.StringToColor(color);
            }

            throw new Exception($"{nameof(GetWindowsSystemColor)} failed to get registry key value for setting: {uiElement}.");
        }

        public static void SetWindowsSystemColor(string uiElement, Color color)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey_WindowsColors, true))
            {
                if (key != null)
                {
                    key.SetValue(uiElement, ColorHelper.ColorToString(color));
                }
            }
        }



    }
}
