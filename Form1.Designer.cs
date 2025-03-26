namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CalculateButton = new Button();
            OutputBox = new RichTextBox();
            SuspendLayout();
            // 
            // CalculateButton
            // 
            CalculateButton.Location = new Point(14, 283);
            CalculateButton.Margin = new Padding(3, 4, 3, 4);
            CalculateButton.Name = "CalculateButton";
            CalculateButton.Size = new Size(433, 31);
            CalculateButton.TabIndex = 0;
            CalculateButton.Text = "Вычислить";
            CalculateButton.UseVisualStyleBackColor = true;
            CalculateButton.Click += CalculateButton_Click;
            // 
            // OutputBox
            // 
            OutputBox.Location = new Point(14, 16);
            OutputBox.Margin = new Padding(3, 4, 3, 4);
            OutputBox.Name = "OutputBox";
            OutputBox.Size = new Size(433, 255);
            OutputBox.TabIndex = 1;
            OutputBox.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(462, 327);
            Controls.Add(OutputBox);
            Controls.Add(CalculateButton);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Заправки";
            ResumeLayout(false);
        }

        #endregion

        private Button CalculateButton;
        private RichTextBox OutputBox;
        private TextBox queueTextBox;
        private Label label1;
    }
}
