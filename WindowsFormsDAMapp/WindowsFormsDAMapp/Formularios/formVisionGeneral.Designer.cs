namespace WindowsFormsDAMapp
{
    partial class formVisionGeneral
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Arqueros",
            "0",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ballesteros",
            "0",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Piqueros",
            "0",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Caballeros",
            "0",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Paladines",
            "0",
            "0"}, -1);
            this.cbx_pueblos = new System.Windows.Forms.ComboBox();
            this.lab_Pueblos = new System.Windows.Forms.Label();
            this.btn_visionGeneral = new System.Windows.Forms.Button();
            this.btn_Clasificacion = new System.Windows.Forms.Button();
            this.btn_mensajes = new System.Windows.Forms.Button();
            this.btn_mapa = new System.Windows.Forms.Button();
            this.btn_movimientos = new System.Windows.Forms.Button();
            this.btn_reclutamiento = new System.Windows.Forms.Button();
            this.lab_tropas = new System.Windows.Forms.Label();
            this.lsv_tropas = new System.Windows.Forms.ListView();
            this.col_tropas = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_cantidadPropia = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_cantidadExterna = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lab_poblacion = new System.Windows.Forms.Label();
            this.lsv_entrantes = new System.Windows.Forms.ListView();
            this.col_puebloOrigen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_tiempoRestante = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_horaLlegada = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_tipoMovimiento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lab_movimientosEntrantes = new System.Windows.Forms.Label();
            this.lab_movimientosSalientes = new System.Windows.Forms.Label();
            this.lsv_Salientes = new System.Windows.Forms.ListView();
            this.col_destino = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_tiempoRestante2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_horaLlegada2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_tipo2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_volver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbx_pueblos
            // 
            this.cbx_pueblos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_pueblos.FormattingEnabled = true;
            this.cbx_pueblos.Location = new System.Drawing.Point(71, 58);
            this.cbx_pueblos.Name = "cbx_pueblos";
            this.cbx_pueblos.Size = new System.Drawing.Size(94, 21);
            this.cbx_pueblos.TabIndex = 18;
            this.cbx_pueblos.SelectedIndexChanged += new System.EventHandler(this.cbx_pueblos_SelectedIndexChanged);
            // 
            // lab_Pueblos
            // 
            this.lab_Pueblos.AutoSize = true;
            this.lab_Pueblos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_Pueblos.Location = new System.Drawing.Point(7, 59);
            this.lab_Pueblos.Name = "lab_Pueblos";
            this.lab_Pueblos.Size = new System.Drawing.Size(58, 16);
            this.lab_Pueblos.TabIndex = 17;
            this.lab_Pueblos.Text = "Pueblos";
            // 
            // btn_visionGeneral
            // 
            this.btn_visionGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_visionGeneral.Location = new System.Drawing.Point(32, 4);
            this.btn_visionGeneral.Name = "btn_visionGeneral";
            this.btn_visionGeneral.Size = new System.Drawing.Size(101, 34);
            this.btn_visionGeneral.TabIndex = 19;
            this.btn_visionGeneral.Text = "Visión General";
            this.btn_visionGeneral.UseVisualStyleBackColor = true;
            this.btn_visionGeneral.Click += new System.EventHandler(this.btn_visionGeneral_Click);
            // 
            // btn_Clasificacion
            // 
            this.btn_Clasificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Clasificacion.Location = new System.Drawing.Point(460, 4);
            this.btn_Clasificacion.Name = "btn_Clasificacion";
            this.btn_Clasificacion.Size = new System.Drawing.Size(101, 34);
            this.btn_Clasificacion.TabIndex = 20;
            this.btn_Clasificacion.Text = "Clasificación";
            this.btn_Clasificacion.UseVisualStyleBackColor = true;
            this.btn_Clasificacion.Click += new System.EventHandler(this.Btn_Clasificacion_Click);
            // 
            // btn_mensajes
            // 
            this.btn_mensajes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_mensajes.Location = new System.Drawing.Point(567, 4);
            this.btn_mensajes.Name = "btn_mensajes";
            this.btn_mensajes.Size = new System.Drawing.Size(101, 34);
            this.btn_mensajes.TabIndex = 21;
            this.btn_mensajes.Text = "Mensajes";
            this.btn_mensajes.UseVisualStyleBackColor = true;
            this.btn_mensajes.Click += new System.EventHandler(this.Btn_mensajes_Click);
            // 
            // btn_mapa
            // 
            this.btn_mapa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_mapa.Location = new System.Drawing.Point(353, 4);
            this.btn_mapa.Name = "btn_mapa";
            this.btn_mapa.Size = new System.Drawing.Size(101, 34);
            this.btn_mapa.TabIndex = 22;
            this.btn_mapa.Text = "Mapa";
            this.btn_mapa.UseVisualStyleBackColor = true;
            this.btn_mapa.Click += new System.EventHandler(this.btn_mapa_Click);
            // 
            // btn_movimientos
            // 
            this.btn_movimientos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_movimientos.Location = new System.Drawing.Point(246, 4);
            this.btn_movimientos.Name = "btn_movimientos";
            this.btn_movimientos.Size = new System.Drawing.Size(101, 34);
            this.btn_movimientos.TabIndex = 23;
            this.btn_movimientos.Text = "Movimientos";
            this.btn_movimientos.UseVisualStyleBackColor = true;
            this.btn_movimientos.Click += new System.EventHandler(this.btn_movimientos_Click);
            // 
            // btn_reclutamiento
            // 
            this.btn_reclutamiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reclutamiento.Location = new System.Drawing.Point(139, 4);
            this.btn_reclutamiento.Name = "btn_reclutamiento";
            this.btn_reclutamiento.Size = new System.Drawing.Size(101, 34);
            this.btn_reclutamiento.TabIndex = 24;
            this.btn_reclutamiento.Text = "Reclutamiento";
            this.btn_reclutamiento.UseVisualStyleBackColor = true;
            this.btn_reclutamiento.Click += new System.EventHandler(this.btn_reclutamiento_Click);
            // 
            // lab_tropas
            // 
            this.lab_tropas.AutoSize = true;
            this.lab_tropas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_tropas.Location = new System.Drawing.Point(7, 106);
            this.lab_tropas.Name = "lab_tropas";
            this.lab_tropas.Size = new System.Drawing.Size(52, 16);
            this.lab_tropas.TabIndex = 25;
            this.lab_tropas.Text = "Tropas";
            // 
            // lsv_tropas
            // 
            this.lsv_tropas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_tropas,
            this.col_cantidadPropia,
            this.col_cantidadExterna});
            this.lsv_tropas.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lsv_tropas.Location = new System.Drawing.Point(10, 135);
            this.lsv_tropas.Name = "lsv_tropas";
            this.lsv_tropas.Size = new System.Drawing.Size(214, 182);
            this.lsv_tropas.TabIndex = 26;
            this.lsv_tropas.UseCompatibleStateImageBehavior = false;
            this.lsv_tropas.View = System.Windows.Forms.View.Details;
            // 
            // col_tropas
            // 
            this.col_tropas.Text = "Tropas";
            this.col_tropas.Width = 84;
            // 
            // col_cantidadPropia
            // 
            this.col_cantidadPropia.Text = "Propias";
            this.col_cantidadPropia.Width = 66;
            // 
            // col_cantidadExterna
            // 
            this.col_cantidadExterna.Text = "Externas";
            // 
            // lab_poblacion
            // 
            this.lab_poblacion.AutoSize = true;
            this.lab_poblacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_poblacion.Location = new System.Drawing.Point(522, 58);
            this.lab_poblacion.Name = "lab_poblacion";
            this.lab_poblacion.Size = new System.Drawing.Size(160, 16);
            this.lab_poblacion.TabIndex = 27;
            this.lab_poblacion.Text = "Nivel de Población: 10/20";
            // 
            // lsv_entrantes
            // 
            this.lsv_entrantes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_puebloOrigen,
            this.col_tiempoRestante,
            this.col_horaLlegada,
            this.col_tipoMovimiento});
            this.lsv_entrantes.Location = new System.Drawing.Point(270, 96);
            this.lsv_entrantes.Name = "lsv_entrantes";
            this.lsv_entrantes.Size = new System.Drawing.Size(386, 148);
            this.lsv_entrantes.TabIndex = 28;
            this.lsv_entrantes.UseCompatibleStateImageBehavior = false;
            this.lsv_entrantes.View = System.Windows.Forms.View.Details;
            // 
            // col_puebloOrigen
            // 
            this.col_puebloOrigen.Text = "Pueblo Origen";
            this.col_puebloOrigen.Width = 84;
            // 
            // col_tiempoRestante
            // 
            this.col_tiempoRestante.Text = "Tiempo Restante";
            this.col_tiempoRestante.Width = 98;
            // 
            // col_horaLlegada
            // 
            this.col_horaLlegada.Text = "Hora Llegada";
            this.col_horaLlegada.Width = 140;
            // 
            // col_tipoMovimiento
            // 
            this.col_tipoMovimiento.Text = "Tipo";
            // 
            // lab_movimientosEntrantes
            // 
            this.lab_movimientosEntrantes.AutoSize = true;
            this.lab_movimientosEntrantes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_movimientosEntrantes.Location = new System.Drawing.Point(267, 77);
            this.lab_movimientosEntrantes.Name = "lab_movimientosEntrantes";
            this.lab_movimientosEntrantes.Size = new System.Drawing.Size(143, 16);
            this.lab_movimientosEntrantes.TabIndex = 30;
            this.lab_movimientosEntrantes.Text = "Movimientos Entrantes";
            // 
            // lab_movimientosSalientes
            // 
            this.lab_movimientosSalientes.AutoSize = true;
            this.lab_movimientosSalientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_movimientosSalientes.Location = new System.Drawing.Point(267, 269);
            this.lab_movimientosSalientes.Name = "lab_movimientosSalientes";
            this.lab_movimientosSalientes.Size = new System.Drawing.Size(133, 16);
            this.lab_movimientosSalientes.TabIndex = 31;
            this.lab_movimientosSalientes.Text = "Movimientos Salidas";
            // 
            // lsv_Salientes
            // 
            this.lsv_Salientes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_destino,
            this.col_tiempoRestante2,
            this.col_horaLlegada2,
            this.col_tipo2});
            this.lsv_Salientes.Location = new System.Drawing.Point(270, 288);
            this.lsv_Salientes.Name = "lsv_Salientes";
            this.lsv_Salientes.Size = new System.Drawing.Size(386, 148);
            this.lsv_Salientes.TabIndex = 32;
            this.lsv_Salientes.UseCompatibleStateImageBehavior = false;
            this.lsv_Salientes.View = System.Windows.Forms.View.Details;
            // 
            // col_destino
            // 
            this.col_destino.Text = "Pueblo Destino";
            this.col_destino.Width = 84;
            // 
            // col_tiempoRestante2
            // 
            this.col_tiempoRestante2.Text = "Tiempo Restante";
            this.col_tiempoRestante2.Width = 93;
            // 
            // col_horaLlegada2
            // 
            this.col_horaLlegada2.Text = "Hora Llegada";
            this.col_horaLlegada2.Width = 145;
            // 
            // col_tipo2
            // 
            this.col_tipo2.Text = "Tipo";
            // 
            // btn_volver
            // 
            this.btn_volver.Location = new System.Drawing.Point(588, 445);
            this.btn_volver.Name = "btn_volver";
            this.btn_volver.Size = new System.Drawing.Size(94, 23);
            this.btn_volver.TabIndex = 33;
            this.btn_volver.Text = "Cerrar Sesion";
            this.btn_volver.UseVisualStyleBackColor = true;
            this.btn_volver.Click += new System.EventHandler(this.btn_volver_Click);
            // 
            // formVisionGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 480);
            this.Controls.Add(this.btn_volver);
            this.Controls.Add(this.lsv_Salientes);
            this.Controls.Add(this.lab_movimientosSalientes);
            this.Controls.Add(this.lab_movimientosEntrantes);
            this.Controls.Add(this.lsv_entrantes);
            this.Controls.Add(this.lab_poblacion);
            this.Controls.Add(this.lsv_tropas);
            this.Controls.Add(this.lab_tropas);
            this.Controls.Add(this.btn_reclutamiento);
            this.Controls.Add(this.btn_movimientos);
            this.Controls.Add(this.btn_mapa);
            this.Controls.Add(this.btn_mensajes);
            this.Controls.Add(this.btn_Clasificacion);
            this.Controls.Add(this.btn_visionGeneral);
            this.Controls.Add(this.cbx_pueblos);
            this.Controls.Add(this.lab_Pueblos);
            this.Name = "formVisionGeneral";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vision General";
            this.Load += new System.EventHandler(this.FormVisionGeneral_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbx_pueblos;
        private System.Windows.Forms.Label lab_Pueblos;
        private System.Windows.Forms.Button btn_visionGeneral;
        private System.Windows.Forms.Button btn_Clasificacion;
        private System.Windows.Forms.Button btn_mensajes;
        private System.Windows.Forms.Button btn_mapa;
        private System.Windows.Forms.Button btn_movimientos;
        private System.Windows.Forms.Button btn_reclutamiento;
        private System.Windows.Forms.Label lab_tropas;
        private System.Windows.Forms.ListView lsv_tropas;
        private System.Windows.Forms.ColumnHeader col_tropas;
        private System.Windows.Forms.ColumnHeader col_cantidadPropia;
        private System.Windows.Forms.Label lab_poblacion;
        private System.Windows.Forms.ListView lsv_entrantes;
        private System.Windows.Forms.ColumnHeader col_puebloOrigen;
        private System.Windows.Forms.ColumnHeader col_tiempoRestante;
        private System.Windows.Forms.ColumnHeader col_horaLlegada;
        private System.Windows.Forms.ColumnHeader col_tipoMovimiento;
        private System.Windows.Forms.Label lab_movimientosEntrantes;
        private System.Windows.Forms.Label lab_movimientosSalientes;
        private System.Windows.Forms.ListView lsv_Salientes;
        private System.Windows.Forms.ColumnHeader col_destino;
        private System.Windows.Forms.ColumnHeader col_tiempoRestante2;
        private System.Windows.Forms.ColumnHeader col_horaLlegada2;
        private System.Windows.Forms.ColumnHeader col_tipo2;
        private System.Windows.Forms.Button btn_volver;
        private System.Windows.Forms.ColumnHeader col_cantidadExterna;
    }
}