namespace WindowsFormsDAMapp
{
    partial class formBandejaEntrada
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
            this.btn_reclutamiento = new System.Windows.Forms.Button();
            this.btn_movimientos = new System.Windows.Forms.Button();
            this.btn_mapa = new System.Windows.Forms.Button();
            this.btn_mensajes = new System.Windows.Forms.Button();
            this.btn_Clasificacion = new System.Windows.Forms.Button();
            this.btn_visionGeneral = new System.Windows.Forms.Button();
            this.lsv_mensajes = new System.Windows.Forms.ListView();
            this.col_asunto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_jugador = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_fecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_redactar = new System.Windows.Forms.Button();
            this.btn_volver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_reclutamiento
            // 
            this.btn_reclutamiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reclutamiento.Location = new System.Drawing.Point(119, 12);
            this.btn_reclutamiento.Name = "btn_reclutamiento";
            this.btn_reclutamiento.Size = new System.Drawing.Size(101, 34);
            this.btn_reclutamiento.TabIndex = 48;
            this.btn_reclutamiento.Text = "Reclutamiento";
            this.btn_reclutamiento.UseVisualStyleBackColor = true;
            this.btn_reclutamiento.Click += new System.EventHandler(this.Btn_reclutamiento_Click);
            // 
            // btn_movimientos
            // 
            this.btn_movimientos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_movimientos.Location = new System.Drawing.Point(226, 12);
            this.btn_movimientos.Name = "btn_movimientos";
            this.btn_movimientos.Size = new System.Drawing.Size(101, 34);
            this.btn_movimientos.TabIndex = 47;
            this.btn_movimientos.Text = "Movimientos";
            this.btn_movimientos.UseVisualStyleBackColor = true;
            this.btn_movimientos.Click += new System.EventHandler(this.btn_movimientos_Click);
            // 
            // btn_mapa
            // 
            this.btn_mapa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_mapa.Location = new System.Drawing.Point(333, 12);
            this.btn_mapa.Name = "btn_mapa";
            this.btn_mapa.Size = new System.Drawing.Size(101, 34);
            this.btn_mapa.TabIndex = 46;
            this.btn_mapa.Text = "Mapa";
            this.btn_mapa.UseVisualStyleBackColor = true;
            this.btn_mapa.Click += new System.EventHandler(this.btn_mapa_Click);
            // 
            // btn_mensajes
            // 
            this.btn_mensajes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_mensajes.Location = new System.Drawing.Point(547, 12);
            this.btn_mensajes.Name = "btn_mensajes";
            this.btn_mensajes.Size = new System.Drawing.Size(101, 34);
            this.btn_mensajes.TabIndex = 45;
            this.btn_mensajes.Text = "Mensajes";
            this.btn_mensajes.UseVisualStyleBackColor = true;
            this.btn_mensajes.Click += new System.EventHandler(this.Btn_mensajes_Click);
            // 
            // btn_Clasificacion
            // 
            this.btn_Clasificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Clasificacion.Location = new System.Drawing.Point(440, 12);
            this.btn_Clasificacion.Name = "btn_Clasificacion";
            this.btn_Clasificacion.Size = new System.Drawing.Size(101, 34);
            this.btn_Clasificacion.TabIndex = 44;
            this.btn_Clasificacion.Text = "Clasificación";
            this.btn_Clasificacion.UseVisualStyleBackColor = true;
            this.btn_Clasificacion.Click += new System.EventHandler(this.Btn_Clasificacion_Click);
            // 
            // btn_visionGeneral
            // 
            this.btn_visionGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_visionGeneral.Location = new System.Drawing.Point(12, 12);
            this.btn_visionGeneral.Name = "btn_visionGeneral";
            this.btn_visionGeneral.Size = new System.Drawing.Size(101, 34);
            this.btn_visionGeneral.TabIndex = 43;
            this.btn_visionGeneral.Text = "Visión General";
            this.btn_visionGeneral.UseVisualStyleBackColor = true;
            this.btn_visionGeneral.Click += new System.EventHandler(this.Btn_visionGeneral_Click);
            // 
            // lsv_mensajes
            // 
            this.lsv_mensajes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_asunto,
            this.col_jugador,
            this.col_fecha});
            this.lsv_mensajes.Location = new System.Drawing.Point(12, 62);
            this.lsv_mensajes.MultiSelect = false;
            this.lsv_mensajes.Name = "lsv_mensajes";
            this.lsv_mensajes.Size = new System.Drawing.Size(636, 296);
            this.lsv_mensajes.TabIndex = 51;
            this.lsv_mensajes.UseCompatibleStateImageBehavior = false;
            this.lsv_mensajes.View = System.Windows.Forms.View.Details;
            this.lsv_mensajes.DoubleClick += new System.EventHandler(this.Lsv_mensajes_DoubleClick);
            // 
            // col_asunto
            // 
            this.col_asunto.Text = "Asunto";
            this.col_asunto.Width = 312;
            // 
            // col_jugador
            // 
            this.col_jugador.Text = "Jugador";
            this.col_jugador.Width = 174;
            // 
            // col_fecha
            // 
            this.col_fecha.Text = "Fecha";
            this.col_fecha.Width = 144;
            // 
            // btn_redactar
            // 
            this.btn_redactar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_redactar.Location = new System.Drawing.Point(538, 382);
            this.btn_redactar.Name = "btn_redactar";
            this.btn_redactar.Size = new System.Drawing.Size(110, 34);
            this.btn_redactar.TabIndex = 52;
            this.btn_redactar.Text = "Nuevo Mensaje";
            this.btn_redactar.UseVisualStyleBackColor = true;
            this.btn_redactar.Click += new System.EventHandler(this.Btn_redactar_Click);
            // 
            // btn_volver
            // 
            this.btn_volver.Location = new System.Drawing.Point(12, 393);
            this.btn_volver.Name = "btn_volver";
            this.btn_volver.Size = new System.Drawing.Size(94, 23);
            this.btn_volver.TabIndex = 74;
            this.btn_volver.Text = "Cerrar Sesion";
            this.btn_volver.UseVisualStyleBackColor = true;
            this.btn_volver.Click += new System.EventHandler(this.Btn_volver_Click);
            // 
            // formBandejaEntrada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 428);
            this.Controls.Add(this.btn_volver);
            this.Controls.Add(this.btn_redactar);
            this.Controls.Add(this.lsv_mensajes);
            this.Controls.Add(this.btn_reclutamiento);
            this.Controls.Add(this.btn_movimientos);
            this.Controls.Add(this.btn_mapa);
            this.Controls.Add(this.btn_mensajes);
            this.Controls.Add(this.btn_Clasificacion);
            this.Controls.Add(this.btn_visionGeneral);
            this.Name = "formBandejaEntrada";
            this.Text = "Bandeja de Entrada";
            this.Load += new System.EventHandler(this.FormBandejaEntrada_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_reclutamiento;
        private System.Windows.Forms.Button btn_movimientos;
        private System.Windows.Forms.Button btn_mapa;
        private System.Windows.Forms.Button btn_mensajes;
        private System.Windows.Forms.Button btn_Clasificacion;
        private System.Windows.Forms.Button btn_visionGeneral;
        private System.Windows.Forms.ListView lsv_mensajes;
        private System.Windows.Forms.ColumnHeader col_asunto;
        private System.Windows.Forms.ColumnHeader col_jugador;
        private System.Windows.Forms.ColumnHeader col_fecha;
        private System.Windows.Forms.Button btn_redactar;
        private System.Windows.Forms.Button btn_volver;
    }
}