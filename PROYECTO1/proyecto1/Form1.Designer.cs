namespace proyecto1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBoxConsultaIA = new TextBox();
            buttonConsultar = new Button();
            textBoxResultadoAI = new TextBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // textBoxConsultaIA
            // 
            textBoxConsultaIA.Location = new Point(88, 371);
            textBoxConsultaIA.Multiline = true;
            textBoxConsultaIA.Name = "textBoxConsultaIA";
            textBoxConsultaIA.Size = new Size(530, 54);
            textBoxConsultaIA.TabIndex = 0;
            // 
            // buttonConsultar
            // 
            buttonConsultar.Location = new Point(624, 370);
            buttonConsultar.Name = "buttonConsultar";
            buttonConsultar.Size = new Size(61, 55);
            buttonConsultar.TabIndex = 1;
            buttonConsultar.Text = " 🢁";
            buttonConsultar.UseVisualStyleBackColor = true;
            buttonConsultar.Click += buttonConsultar_Click;
            // 
            // textBoxResultadoAI
            // 
            textBoxResultadoAI.Cursor = Cursors.IBeam;
            textBoxResultadoAI.Location = new Point(88, 68);
            textBoxResultadoAI.Margin = new Padding(2);
            textBoxResultadoAI.Multiline = true;
            textBoxResultadoAI.Name = "textBoxResultadoAI";
            textBoxResultadoAI.ReadOnly = true;
            textBoxResultadoAI.ScrollBars = ScrollBars.Vertical;
            textBoxResultadoAI.Size = new Size(597, 271);
            textBoxResultadoAI.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(88, 353);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 9;
            label1.Text = "Consultar AI";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(88, 51);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 10;
            label2.Text = "Respuesta: ";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(-2, -2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(805, 455);
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.InactiveCaptionText;
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(286, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(178, 50);
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxResultadoAI);
            Controls.Add(buttonConsultar);
            Controls.Add(textBoxConsultaIA);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxConsultaIA;
        private Button buttonConsultar;
        private TextBox textBoxResultadoAI;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}
