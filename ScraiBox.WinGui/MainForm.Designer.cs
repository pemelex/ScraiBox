namespace ScraiBox.WinGui
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblPath = new Label();
            btnBrowse = new Button();
            splitMain = new SplitContainer();
            grpInterceptor = new GroupBox();
            txtAiInput = new RichTextBox();
            btnProcess = new Button();
            grpLogs = new GroupBox();
            lstLogs = new ListBox();
            grpActions = new GroupBox();
            comboUseCase = new ComboBox();
            txtTarget = new TextBox();
            btnRunUseCase = new Button();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            grpInterceptor.SuspendLayout();
            grpLogs.SuspendLayout();
            grpActions.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblPath);
            pnlTop.Controls.Add(btnBrowse);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(900, 60);
            pnlTop.TabIndex = 1;
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Location = new Point(140, 23);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(184, 25);
            lblPath.TabIndex = 0;
            lblPath.Text = "Select root directory...";
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(12, 15);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(120, 30);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "📁 Browse Project";
            btnBrowse.Click += btnBrowse_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 60);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(grpInterceptor);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(grpLogs);
            splitMain.Panel2.Controls.Add(grpActions);
            splitMain.Size = new Size(900, 540);
            splitMain.SplitterDistance = 472;
            splitMain.TabIndex = 0;
            // 
            // grpInterceptor
            // 
            grpInterceptor.Controls.Add(txtAiInput);
            grpInterceptor.Controls.Add(btnProcess);
            grpInterceptor.Dock = DockStyle.Fill;
            grpInterceptor.Location = new Point(0, 0);
            grpInterceptor.Name = "grpInterceptor";
            grpInterceptor.Size = new Size(472, 540);
            grpInterceptor.TabIndex = 0;
            grpInterceptor.TabStop = false;
            grpInterceptor.Text = "AI Response Interceptor";
            // 
            // txtAiInput
            // 
            txtAiInput.Dock = DockStyle.Fill;
            txtAiInput.Font = new Font("Cascadia Code", 9F);
            txtAiInput.Location = new Point(3, 27);
            txtAiInput.Name = "txtAiInput";
            txtAiInput.Size = new Size(466, 470);
            txtAiInput.TabIndex = 0;
            txtAiInput.Text = "";
            // 
            // btnProcess
            // 
            btnProcess.Dock = DockStyle.Bottom;
            btnProcess.Location = new Point(3, 497);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(466, 40);
            btnProcess.TabIndex = 1;
            btnProcess.Text = "⚡ Process & Copy to Clipboard";
            btnProcess.Click += btnProcess_Click;
            // 
            // grpLogs
            // 
            grpLogs.Controls.Add(lstLogs);
            grpLogs.Dock = DockStyle.Fill;
            grpLogs.Location = new Point(0, 150);
            grpLogs.Name = "grpLogs";
            grpLogs.Size = new Size(424, 390);
            grpLogs.TabIndex = 0;
            grpLogs.TabStop = false;
            grpLogs.Text = "Activity Log";
            // 
            // lstLogs
            // 
            lstLogs.Dock = DockStyle.Fill;
            lstLogs.Location = new Point(3, 27);
            lstLogs.Name = "lstLogs";
            lstLogs.Size = new Size(418, 360);
            lstLogs.TabIndex = 0;
            // 
            // grpActions
            // 
            grpActions.Controls.Add(comboUseCase);
            grpActions.Controls.Add(txtTarget);
            grpActions.Controls.Add(btnRunUseCase);
            grpActions.Dock = DockStyle.Top;
            grpActions.Location = new Point(0, 0);
            grpActions.Name = "grpActions";
            grpActions.Size = new Size(424, 150);
            grpActions.TabIndex = 1;
            grpActions.TabStop = false;
            grpActions.Text = "🚀 Use Case Actions";
            // 
            // comboUseCase
            // 
            comboUseCase.DropDownStyle = ComboBoxStyle.DropDownList;
            comboUseCase.Items.AddRange(new object[] { "SelfHydration", "BlazorEdit", "DeepContextTracer", "MethodCallTree" });
            comboUseCase.Location = new Point(10, 30);
            comboUseCase.Name = "comboUseCase";
            comboUseCase.Size = new Size(250, 33);
            comboUseCase.TabIndex = 0;
            // 
            // txtTarget
            // 
            txtTarget.Location = new Point(10, 65);
            txtTarget.Name = "txtTarget";
            txtTarget.PlaceholderText = "Target (e.g. Home.razor)";
            txtTarget.Size = new Size(250, 31);
            txtTarget.TabIndex = 1;
            // 
            // btnRunUseCase
            // 
            btnRunUseCase.BackColor = Color.LightGreen;
            btnRunUseCase.Location = new Point(10, 100);
            btnRunUseCase.Name = "btnRunUseCase";
            btnRunUseCase.Size = new Size(250, 35);
            btnRunUseCase.TabIndex = 2;
            btnRunUseCase.Text = "Run Selected Use Case";
            btnRunUseCase.UseVisualStyleBackColor = false;
            btnRunUseCase.Click += btnRunUseCase_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(900, 600);
            Controls.Add(splitMain);
            Controls.Add(pnlTop);
            Name = "MainForm";
            Text = "🔮 ScraiBox Control Center (.NET 10)";
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            grpInterceptor.ResumeLayout(false);
            grpLogs.ResumeLayout(false);
            grpActions.ResumeLayout(false);
            grpActions.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpInterceptor;
        private System.Windows.Forms.RichTextBox txtAiInput;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.ComboBox comboUseCase;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button btnRunUseCase;
        private System.Windows.Forms.GroupBox grpLogs;
        private System.Windows.Forms.ListBox lstLogs;
    }
}