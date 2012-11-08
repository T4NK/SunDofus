namespace SunDofus.Editor
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nouveauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sauvegarderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sauvegarderSousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.affichageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afficherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.erCalqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.èmeCalqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grilleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ligneDeVueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typesDesCasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.solToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.erCalqueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.èmeCalqueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.caseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ligneDeVueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.typesDesCasesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.basesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificationDuFondToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificationDeLaMusiqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificationDeLambianceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seConnecterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seConnecterAuServeurMySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Logger = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.affichageToolStripMenuItem,
            this.modificationToolStripMenuItem,
            this.serverToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1255, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouveauToolStripMenuItem,
            this.ouvrirToolStripMenuItem,
            this.sauvegarderToolStripMenuItem,
            this.sauvegarderSousToolStripMenuItem,
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "Fichier";
            // 
            // nouveauToolStripMenuItem
            // 
            this.nouveauToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem});
            this.nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            this.nouveauToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.nouveauToolStripMenuItem.Text = "Nouveau";
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.mapToolStripMenuItem.Text = "Map";
            this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // ouvrirToolStripMenuItem
            // 
            this.ouvrirToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem1});
            this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ouvrirToolStripMenuItem.Text = "Ouvrir";
            // 
            // mapToolStripMenuItem1
            // 
            this.mapToolStripMenuItem1.Name = "mapToolStripMenuItem1";
            this.mapToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            this.mapToolStripMenuItem1.Text = "Map";
            // 
            // sauvegarderToolStripMenuItem
            // 
            this.sauvegarderToolStripMenuItem.Name = "sauvegarderToolStripMenuItem";
            this.sauvegarderToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sauvegarderToolStripMenuItem.Text = "Sauvegarder";
            this.sauvegarderToolStripMenuItem.Click += new System.EventHandler(this.sauvegarderToolStripMenuItem_Click);
            // 
            // sauvegarderSousToolStripMenuItem
            // 
            this.sauvegarderSousToolStripMenuItem.Name = "sauvegarderSousToolStripMenuItem";
            this.sauvegarderSousToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sauvegarderSousToolStripMenuItem.Text = "Sauvegarder sous...";
            this.sauvegarderSousToolStripMenuItem.Click += new System.EventHandler(this.sauvegarderSousToolStripMenuItem_Click);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.quitterToolStripMenuItem.Text = "Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // affichageToolStripMenuItem
            // 
            this.affichageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.afficherToolStripMenuItem,
            this.informationsToolStripMenuItem});
            this.affichageToolStripMenuItem.Name = "affichageToolStripMenuItem";
            this.affichageToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.affichageToolStripMenuItem.Text = "Affichage";
            // 
            // afficherToolStripMenuItem
            // 
            this.afficherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solToolStripMenuItem,
            this.erCalqueToolStripMenuItem,
            this.èmeCalqueToolStripMenuItem,
            this.grilleToolStripMenuItem});
            this.afficherToolStripMenuItem.Name = "afficherToolStripMenuItem";
            this.afficherToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.afficherToolStripMenuItem.Text = "Afficher";
            // 
            // solToolStripMenuItem
            // 
            this.solToolStripMenuItem.Name = "solToolStripMenuItem";
            this.solToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.solToolStripMenuItem.Text = "Sol";
            this.solToolStripMenuItem.Click += new System.EventHandler(this.solToolStripMenuItem_Click);
            // 
            // erCalqueToolStripMenuItem
            // 
            this.erCalqueToolStripMenuItem.Name = "erCalqueToolStripMenuItem";
            this.erCalqueToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.erCalqueToolStripMenuItem.Text = "1er calque";
            this.erCalqueToolStripMenuItem.Click += new System.EventHandler(this.erCalqueToolStripMenuItem_Click);
            // 
            // èmeCalqueToolStripMenuItem
            // 
            this.èmeCalqueToolStripMenuItem.Name = "èmeCalqueToolStripMenuItem";
            this.èmeCalqueToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.èmeCalqueToolStripMenuItem.Text = "2ème calque";
            this.èmeCalqueToolStripMenuItem.Click += new System.EventHandler(this.èmeCalqueToolStripMenuItem_Click);
            // 
            // grilleToolStripMenuItem
            // 
            this.grilleToolStripMenuItem.Name = "grilleToolStripMenuItem";
            this.grilleToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.grilleToolStripMenuItem.Text = "Grille";
            this.grilleToolStripMenuItem.Click += new System.EventHandler(this.grilleToolStripMenuItem_Click);
            // 
            // informationsToolStripMenuItem
            // 
            this.informationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ligneDeVueToolStripMenuItem,
            this.typesDesCasesToolStripMenuItem});
            this.informationsToolStripMenuItem.Name = "informationsToolStripMenuItem";
            this.informationsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.informationsToolStripMenuItem.Text = "Cases";
            // 
            // ligneDeVueToolStripMenuItem
            // 
            this.ligneDeVueToolStripMenuItem.Name = "ligneDeVueToolStripMenuItem";
            this.ligneDeVueToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.ligneDeVueToolStripMenuItem.Text = "Ligne de vue";
            this.ligneDeVueToolStripMenuItem.Click += new System.EventHandler(this.ligneDeVueToolStripMenuItem_Click);
            // 
            // typesDesCasesToolStripMenuItem
            // 
            this.typesDesCasesToolStripMenuItem.Name = "typesDesCasesToolStripMenuItem";
            this.typesDesCasesToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.typesDesCasesToolStripMenuItem.Text = "Types des cases";
            this.typesDesCasesToolStripMenuItem.Click += new System.EventHandler(this.typesDesCasesToolStripMenuItem_Click);
            // 
            // modificationToolStripMenuItem
            // 
            this.modificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calqueToolStripMenuItem,
            this.caseToolStripMenuItem,
            this.basesToolStripMenuItem});
            this.modificationToolStripMenuItem.Name = "modificationToolStripMenuItem";
            this.modificationToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.modificationToolStripMenuItem.Text = "Modifications";
            // 
            // calqueToolStripMenuItem
            // 
            this.calqueToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solToolStripMenuItem1,
            this.solToolStripMenuItem2,
            this.erCalqueToolStripMenuItem1,
            this.èmeCalqueToolStripMenuItem1});
            this.calqueToolStripMenuItem.Name = "calqueToolStripMenuItem";
            this.calqueToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.calqueToolStripMenuItem.Text = "Calques";
            // 
            // solToolStripMenuItem1
            // 
            this.solToolStripMenuItem1.Name = "solToolStripMenuItem1";
            this.solToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.solToolStripMenuItem1.Text = "Aucune";
            this.solToolStripMenuItem1.Click += new System.EventHandler(this.solToolStripMenuItem1_Click);
            // 
            // solToolStripMenuItem2
            // 
            this.solToolStripMenuItem2.Name = "solToolStripMenuItem2";
            this.solToolStripMenuItem2.Size = new System.Drawing.Size(141, 22);
            this.solToolStripMenuItem2.Text = "Sol";
            this.solToolStripMenuItem2.Click += new System.EventHandler(this.solToolStripMenuItem2_Click);
            // 
            // erCalqueToolStripMenuItem1
            // 
            this.erCalqueToolStripMenuItem1.Name = "erCalqueToolStripMenuItem1";
            this.erCalqueToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.erCalqueToolStripMenuItem1.Text = "1er calque";
            this.erCalqueToolStripMenuItem1.Click += new System.EventHandler(this.erCalqueToolStripMenuItem1_Click);
            // 
            // èmeCalqueToolStripMenuItem1
            // 
            this.èmeCalqueToolStripMenuItem1.Name = "èmeCalqueToolStripMenuItem1";
            this.èmeCalqueToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.èmeCalqueToolStripMenuItem1.Text = "2ème calque";
            this.èmeCalqueToolStripMenuItem1.Click += new System.EventHandler(this.èmeCalqueToolStripMenuItem1_Click);
            // 
            // caseToolStripMenuItem
            // 
            this.caseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ligneDeVueToolStripMenuItem1,
            this.typesDesCasesToolStripMenuItem1});
            this.caseToolStripMenuItem.Name = "caseToolStripMenuItem";
            this.caseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.caseToolStripMenuItem.Text = "Cases";
            // 
            // ligneDeVueToolStripMenuItem1
            // 
            this.ligneDeVueToolStripMenuItem1.Name = "ligneDeVueToolStripMenuItem1";
            this.ligneDeVueToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.ligneDeVueToolStripMenuItem1.Text = "Ligne de vue";
            this.ligneDeVueToolStripMenuItem1.Click += new System.EventHandler(this.ligneDeVueToolStripMenuItem1_Click);
            // 
            // typesDesCasesToolStripMenuItem1
            // 
            this.typesDesCasesToolStripMenuItem1.Name = "typesDesCasesToolStripMenuItem1";
            this.typesDesCasesToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.typesDesCasesToolStripMenuItem1.Text = "Types des cases";
            this.typesDesCasesToolStripMenuItem1.Click += new System.EventHandler(this.typesDesCasesToolStripMenuItem1_Click);
            // 
            // basesToolStripMenuItem
            // 
            this.basesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modificationDuFondToolStripMenuItem,
            this.modificationDeLaMusiqueToolStripMenuItem,
            this.modificationDeLambianceToolStripMenuItem});
            this.basesToolStripMenuItem.Name = "basesToolStripMenuItem";
            this.basesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.basesToolStripMenuItem.Text = "Bases";
            // 
            // modificationDuFondToolStripMenuItem
            // 
            this.modificationDuFondToolStripMenuItem.Name = "modificationDuFondToolStripMenuItem";
            this.modificationDuFondToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.modificationDuFondToolStripMenuItem.Text = "Modification du fond";
            this.modificationDuFondToolStripMenuItem.Click += new System.EventHandler(this.modificationDuFondToolStripMenuItem_Click);
            // 
            // modificationDeLaMusiqueToolStripMenuItem
            // 
            this.modificationDeLaMusiqueToolStripMenuItem.Name = "modificationDeLaMusiqueToolStripMenuItem";
            this.modificationDeLaMusiqueToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.modificationDeLaMusiqueToolStripMenuItem.Text = "Modification de la musique";
            // 
            // modificationDeLambianceToolStripMenuItem
            // 
            this.modificationDeLambianceToolStripMenuItem.Name = "modificationDeLambianceToolStripMenuItem";
            this.modificationDeLambianceToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.modificationDeLambianceToolStripMenuItem.Text = "Modification de l\'ambiance";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seConnecterToolStripMenuItem,
            this.seConnecterAuServeurMySQLToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.serverToolStripMenuItem.Text = "Serveurs";
            // 
            // seConnecterToolStripMenuItem
            // 
            this.seConnecterToolStripMenuItem.Name = "seConnecterToolStripMenuItem";
            this.seConnecterToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.seConnecterToolStripMenuItem.Text = "Se connecter au serveur de jeu";
            this.seConnecterToolStripMenuItem.Click += new System.EventHandler(this.seConnecterToolStripMenuItem_Click);
            // 
            // seConnecterAuServeurMySQLToolStripMenuItem
            // 
            this.seConnecterAuServeurMySQLToolStripMenuItem.Name = "seConnecterAuServeurMySQLToolStripMenuItem";
            this.seConnecterAuServeurMySQLToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.seConnecterAuServeurMySQLToolStripMenuItem.Text = "Se connecter au serveur MySQL";
            this.seConnecterAuServeurMySQLToolStripMenuItem.Click += new System.EventHandler(this.seConnecterAuServeurMySQLToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1061, 626);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(1079, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(164, 174);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // Logger
            // 
            this.Logger.BackColor = System.Drawing.SystemColors.MenuBar;
            this.Logger.Location = new System.Drawing.Point(1079, 479);
            this.Logger.Multiline = true;
            this.Logger.Name = "Logger";
            this.Logger.ReadOnly = true;
            this.Logger.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Logger.Size = new System.Drawing.Size(164, 105);
            this.Logger.TabIndex = 6;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1112, 590);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Défis possibles";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(1112, 613);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(123, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Agressions possibles";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(1112, 636);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(96, 17);
            this.checkBox3.TabIndex = 9;
            this.checkBox3.Text = "Map extérieure";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(1079, 207);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 266);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 661);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Logger);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SunDofus.Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nouveauToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ouvrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sauvegarderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sauvegarderSousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem affichageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afficherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem erCalqueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem èmeCalqueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grilleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ligneDeVueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typesDesCasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calqueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem solToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem erCalqueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem èmeCalqueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem caseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ligneDeVueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem typesDesCasesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem basesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificationDuFondToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificationDeLaMusiqueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificationDeLambianceToolStripMenuItem;
        public System.Windows.Forms.TextBox Logger;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        public System.Windows.Forms.TreeView treeView1;
        public System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seConnecterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seConnecterAuServeurMySQLToolStripMenuItem;
    }
}

