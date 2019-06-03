namespace WindowsFormsDAMapp
{
    partial class formInicioSesion
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
            this.lab_contrasñea = new System.Windows.Forms.Label();
            this.lab_nombreUsuario = new System.Windows.Forms.Label();
            this.tbx_nombreUsuario = new System.Windows.Forms.TextBox();
            this.tbx_contraseña = new System.Windows.Forms.TextBox();
            this.btn_iniciarSesion = new System.Windows.Forms.Button();
            this.btn_crearCuenta = new System.Windows.Forms.Button();
            this.lab_partida = new System.Windows.Forms.Label();
            this.btn_volver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lab_contrasñea
            // 
            this.lab_contrasñea.AutoSize = true;
            this.lab_contrasñea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_contrasñea.Location = new System.Drawing.Point(35, 94);
            this.lab_contrasñea.Name = "lab_contrasñea";
            this.lab_contrasñea.Size = new System.Drawing.Size(77, 16);
            this.lab_contrasñea.TabIndex = 15;
            this.lab_contrasñea.Text = "Contraseña";
            // 
            // lab_nombreUsuario
            // 
            this.lab_nombreUsuario.AutoSize = true;
            this.lab_nombreUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_nombreUsuario.Location = new System.Drawing.Point(35, 50);
            this.lab_nombreUsuario.Name = "lab_nombreUsuario";
            this.lab_nombreUsuario.Size = new System.Drawing.Size(107, 16);
            this.lab_nombreUsuario.TabIndex = 14;
            this.lab_nombreUsuario.Text = "Nombre Usuario";
            // 
            // tbx_nombreUsuario
            // 
            this.tbx_nombreUsuario.Location = new System.Drawing.Point(148, 49);
            this.tbx_nombreUsuario.Name = "tbx_nombreUsuario";
            this.tbx_nombreUsuario.Size = new System.Drawing.Size(173, 20);
            this.tbx_nombreUsuario.TabIndex = 16;
            // 
            // tbx_contraseña
            // 
            this.tbx_contraseña.Location = new System.Drawing.Point(148, 93);
            this.tbx_contraseña.Name = "tbx_contraseña";
            this.tbx_contraseña.PasswordChar = '*';
            this.tbx_contraseña.Size = new System.Drawing.Size(173, 20);
            this.tbx_contraseña.TabIndex = 17;
            // 
            // btn_iniciarSesion
            // 
            this.btn_iniciarSesion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_iniciarSesion.Location = new System.Drawing.Point(38, 156);
            this.btn_iniciarSesion.Name = "btn_iniciarSesion";
            this.btn_iniciarSesion.Size = new System.Drawing.Size(88, 41);
            this.btn_iniciarSesion.TabIndex = 18;
            this.btn_iniciarSesion.Text = "Conectarse";
            this.btn_iniciarSesion.UseVisualStyleBackColor = true;
            this.btn_iniciarSesion.Click += new System.EventHandler(this.Btn_iniciarSesion_Click);
            // 
            // btn_crearCuenta
            // 
            this.btn_crearCuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_crearCuenta.Location = new System.Drawing.Point(233, 156);
            this.btn_crearCuenta.Name = "btn_crearCuenta";
            this.btn_crearCuenta.Size = new System.Drawing.Size(88, 41);
            this.btn_crearCuenta.TabIndex = 19;
            this.btn_crearCuenta.Text = "Crear cuenta";
            this.btn_crearCuenta.UseVisualStyleBackColor = true;
            this.btn_crearCuenta.Click += new System.EventHandler(this.Btn_crearCuenta_Click);
            // 
            // lab_partida
            // 
            this.lab_partida.AutoSize = true;
            this.lab_partida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_partida.Location = new System.Drawing.Point(12, 9);
            this.lab_partida.Name = "lab_partida";
            this.lab_partida.Size = new System.Drawing.Size(64, 16);
            this.lab_partida.TabIndex = 20;
            this.lab_partida.Text = "Partida: 1";
            // 
            // btn_volver
            // 
            this.btn_volver.Location = new System.Drawing.Point(266, 6);
            this.btn_volver.Name = "btn_volver";
            this.btn_volver.Size = new System.Drawing.Size(75, 23);
            this.btn_volver.TabIndex = 21;
            this.btn_volver.Text = "Volver";
            this.btn_volver.UseVisualStyleBackColor = true;
            this.btn_volver.Click += new System.EventHandler(this.Btn_volver_Click);
            // 
            // formInicioSesion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 239);
            this.Controls.Add(this.btn_volver);
            this.Controls.Add(this.lab_partida);
            this.Controls.Add(this.btn_crearCuenta);
            this.Controls.Add(this.btn_iniciarSesion);
            this.Controls.Add(this.tbx_contraseña);
            this.Controls.Add(this.tbx_nombreUsuario);
            this.Controls.Add(this.lab_contrasñea);
            this.Controls.Add(this.lab_nombreUsuario);
            this.Name = "formInicioSesion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio Sesion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formInicioSesion_FormClosing);
            this.Load += new System.EventHandler(this.formInicioSesion_Load);
            this.LocationChanged += new System.EventHandler(this.FormInicioSesion_LocationChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_contrasñea;
        private System.Windows.Forms.Label lab_nombreUsuario;
        private System.Windows.Forms.TextBox tbx_nombreUsuario;
        private System.Windows.Forms.TextBox tbx_contraseña;
        private System.Windows.Forms.Button btn_iniciarSesion;
        private System.Windows.Forms.Button btn_crearCuenta;
        private System.Windows.Forms.Label lab_partida;
        private System.Windows.Forms.Button btn_volver;
    }
}