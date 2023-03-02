namespace DS2_SOTFS_ModdingToolbox
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            webView = new Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView();
            SuspendLayout();
            // 
            // webView
            // 
            webView.BackColor = System.Drawing.Color.Black;
            webView.Dock = System.Windows.Forms.DockStyle.Fill;
            webView.ForeColor = System.Drawing.Color.White;
            webView.HostPage = "wwwroot\\index.html";
            webView.Location = new System.Drawing.Point(0, 0);
            webView.Margin = new System.Windows.Forms.Padding(0);
            webView.Name = "webView";
            webView.Size = new System.Drawing.Size(519, 496);
            webView.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.Black;
            ClientSize = new System.Drawing.Size(519, 496);
            Controls.Add(webView);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DS2 Editor Toolset";
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView webView;
    }
}