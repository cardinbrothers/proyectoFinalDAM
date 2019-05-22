namespace WindowsFormsDAMapp
{
    partial class frm_MenuPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lsv_PartidasActivas = new System.Windows.Forms.ListView();
            this.col_Partida = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Velocidad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_tiempoRestante = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_plazasLibres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lab_velocidad = new System.Windows.Forms.Label();
            this.lab_PartidasActivas = new System.Windows.Forms.Label();
            this.btnCrearPartida = new System.Windows.Forms.Button();
            this.lab_crearPartida = new System.Windows.Forms.Label();
            this.lab_duracion = new System.Windows.Forms.Label();
            this.lab_limiteJugadores = new System.Windows.Forms.Label();
            this.lab_limitePoblacion = new System.Windows.Forms.Label();
            this.cbx_velocidad = new System.Windows.Forms.ComboBox();
            this.cbx_Durarcion = new System.Windows.Forms.ComboBox();
            this.cbx_limiteJugadores = new System.Windows.Forms.ComboBox();
            this.cbx_limitePoblacion = new System.Windows.Forms.ComboBox();
            this.btn_recargar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lsv_PartidasActivas
            // 
            this.lsv_PartidasActivas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Partida,
            this.col_Velocidad,
            this.col_tiempoRestante,
            this.col_plazasLibres});
            this.lsv_PartidasActivas.Location = new System.Drawing.Point(12, 54);
            this.lsv_PartidasActivas.Name = "lsv_PartidasActivas";
            this.lsv_PartidasActivas.Size = new System.Drawing.Size(337, 277);
            this.lsv_PartidasActivas.TabIndex = 0;
            this.lsv_PartidasActivas.UseCompatibleStateImageBehavior = false;
            this.lsv_PartidasActivas.View = System.Windows.Forms.View.Details;
            // 
            // col_Partida
            // 
            this.col_Partida.Text = "Partida";
            // 
            // col_Velocidad
            // 
            this.col_Velocidad.Text = "Velocidad";
            // 
            // col_tiempoRestante
            // 
            this.col_tiempoRestante.Text = "Tiempo Restante";
            this.col_tiempoRestante.Width = 136;
            // 
            // col_plazasLibres
            // 
            this.col_plazasLibres.Text = "Plazas Libres";
            this.col_plazasLibres.Width = 77;
            // 
            // lab_velocidad
            // 
            this.lab_velocidad.AutoSize = true;
            this.lab_velocidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_velocidad.Location = new System.Drawing.Point(389, 77);
            this.lab_velocidad.Name = "lab_velocidad";
            this.lab_velocidad.Size = new System.Drawing.Size(70, 16);
            this.lab_velocidad.TabIndex = 1;
            this.lab_velocidad.Text = "Velocidad";
            // 
            // lab_PartidasActivas
            // 
            this.lab_PartidasActivas.AutoSize = true;
            this.lab_PartidasActivas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_PartidasActivas.Location = new System.Drawing.Point(9, 29);
            this.lab_PartidasActivas.Name = "lab_PartidasActivas";
            this.lab_PartidasActivas.Size = new System.Drawing.Size(121, 16);
            this.lab_PartidasActivas.TabIndex = 2;
            this.lab_PartidasActivas.Text = "Partidas Activas";
            // 
            // btnCrearPartida
            // 
            this.btnCrearPartida.Location = new System.Drawing.Point(648, 265);
            this.btnCrearPartida.Name = "btnCrearPartida";
            this.btnCrearPartida.Size = new System.Drawing.Size(91, 36);
            this.btnCrearPartida.TabIndex = 4;
            this.btnCrearPartida.Text = "Partida Nueva";
            this.btnCrearPartida.UseVisualStyleBackColor = true;
            // 
            // lab_crearPartida
            // 
            this.lab_crearPartida.AutoSize = true;
            this.lab_crearPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_crearPartida.Location = new System.Drawing.Point(493, 29);
            this.lab_crearPartida.Name = "lab_crearPartida";
            this.lab_crearPartida.Size = new System.Drawing.Size(149, 16);
            this.lab_crearPartida.TabIndex = 12;
            this.lab_crearPartida.Text = "Crear Partida Nueva";
            // 
            // lab_duracion
            // 
            this.lab_duracion.AutoSize = true;
            this.lab_duracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_duracion.Location = new System.Drawing.Point(389, 121);
            this.lab_duracion.Name = "lab_duracion";
            this.lab_duracion.Size = new System.Drawing.Size(62, 16);
            this.lab_duracion.TabIndex = 13;
            this.lab_duracion.Text = "Duracion";
            // 
            // lab_limiteJugadores
            // 
            this.lab_limiteJugadores.AutoSize = true;
            this.lab_limiteJugadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_limiteJugadores.Location = new System.Drawing.Point(389, 165);
            this.lab_limiteJugadores.Name = "lab_limiteJugadores";
            this.lab_limiteJugadores.Size = new System.Drawing.Size(126, 16);
            this.lab_limiteJugadores.TabIndex = 14;
            this.lab_limiteJugadores.Text = "Limite de jugadores";
            // 
            // lab_limitePoblacion
            // 
            this.lab_limitePoblacion.AutoSize = true;
            this.lab_limitePoblacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_limitePoblacion.Location = new System.Drawing.Point(389, 211);
            this.lab_limitePoblacion.Name = "lab_limitePoblacion";
            this.lab_limitePoblacion.Size = new System.Drawing.Size(126, 16);
            this.lab_limitePoblacion.TabIndex = 15;
            this.lab_limitePoblacion.Text = "Limite de Poblacion";
            // 
            // cbx_velocidad
            // 
            this.cbx_velocidad.FormattingEnabled = true;
            this.cbx_velocidad.Items.AddRange(new object[] {
            "x1",
            "x2",
            "x4",
            "x8",
            "x16"});
            this.cbx_velocidad.Location = new System.Drawing.Point(550, 72);
            this.cbx_velocidad.Name = "cbx_velocidad";
            this.cbx_velocidad.Size = new System.Drawing.Size(189, 21);
            this.cbx_velocidad.TabIndex = 16;
            // 
            // cbx_Durarcion
            // 
            this.cbx_Durarcion.FormattingEnabled = true;
            this.cbx_Durarcion.Items.AddRange(new object[] {
            "1 min",
            "5 min",
            "20 min",
            "1 hora",
            "2 horas",
            "6 horas"});
            this.cbx_Durarcion.Location = new System.Drawing.Point(550, 120);
            this.cbx_Durarcion.Name = "cbx_Durarcion";
            this.cbx_Durarcion.Size = new System.Drawing.Size(189, 21);
            this.cbx_Durarcion.TabIndex = 17;
            // 
            // cbx_limiteJugadores
            // 
            this.cbx_limiteJugadores.FormattingEnabled = true;
            this.cbx_limiteJugadores.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16"});
            this.cbx_limiteJugadores.Location = new System.Drawing.Point(550, 164);
            this.cbx_limiteJugadores.Name = "cbx_limiteJugadores";
            this.cbx_limiteJugadores.Size = new System.Drawing.Size(189, 21);
            this.cbx_limiteJugadores.TabIndex = 18;
            // 
            // cbx_limitePoblacion
            // 
            this.cbx_limitePoblacion.FormattingEnabled = true;
            this.cbx_limitePoblacion.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50",
            "100"});
            this.cbx_limitePoblacion.Location = new System.Drawing.Point(550, 210);
            this.cbx_limitePoblacion.Name = "cbx_limitePoblacion";
            this.cbx_limitePoblacion.Size = new System.Drawing.Size(189, 21);
            this.cbx_limitePoblacion.TabIndex = 19;
            // 
            // btn_recargar
            // 
            this.btn_recargar.BackColor = System.Drawing.Color.Transparent;
            this.btn_recargar.BackgroundImage = global::WindowsFormsDAMapp.Properties.Resources.refresh;
            this.btn_recargar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_recargar.Location = new System.Drawing.Point(289, 29);
            this.btn_recargar.Name = "btn_recargar";
            this.btn_recargar.Size = new System.Drawing.Size(24, 23);
            this.btn_recargar.TabIndex = 3;
            this.btn_recargar.UseVisualStyleBackColor = false;
            this.btn_recargar.Click += new System.EventHandler(this.Btn_recargar_Click);
            // 
            // frm_MenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 350);
            this.Controls.Add(this.cbx_limitePoblacion);
            this.Controls.Add(this.cbx_limiteJugadores);
            this.Controls.Add(this.cbx_Durarcion);
            this.Controls.Add(this.cbx_velocidad);
            this.Controls.Add(this.lab_limitePoblacion);
            this.Controls.Add(this.lab_limiteJugadores);
            this.Controls.Add(this.lab_duracion);
            this.Controls.Add(this.lab_crearPartida);
            this.Controls.Add(this.btnCrearPartida);
            this.Controls.Add(this.btn_recargar);
            this.Controls.Add(this.lab_PartidasActivas);
            this.Controls.Add(this.lab_velocidad);
            this.Controls.Add(this.lsv_PartidasActivas);
            this.Name = "frm_MenuPrincipal";
            this.Text = "Menu Principal";
            this.Load += new System.EventHandler(this.frm_MenuPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lsv_PartidasActivas;
        private System.Windows.Forms.ColumnHeader col_Partida;
        private System.Windows.Forms.ColumnHeader col_Velocidad;
        private System.Windows.Forms.ColumnHeader col_tiempoRestante;
        private System.Windows.Forms.ColumnHeader col_plazasLibres;
        private System.Windows.Forms.Label lab_velocidad;
        private System.Windows.Forms.Label lab_PartidasActivas;
        private System.Windows.Forms.Button btn_recargar;
        private System.Windows.Forms.Button btnCrearPartida;
        private System.Windows.Forms.Label lab_crearPartida;
        private System.Windows.Forms.Label lab_duracion;
        private System.Windows.Forms.Label lab_limiteJugadores;
        private System.Windows.Forms.Label lab_limitePoblacion;
        private System.Windows.Forms.ComboBox cbx_velocidad;
        private System.Windows.Forms.ComboBox cbx_Durarcion;
        private System.Windows.Forms.ComboBox cbx_limiteJugadores;
        private System.Windows.Forms.ComboBox cbx_limitePoblacion;
    }
}

