﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drones
{
    public partial class Form1 : Form
    {
        List<Drone> drones = new List<Drone>();

        public Form1()
        {
            InitializeComponent();
        }

        void RefreshData()
        {
            dataGridView1.RowCount = 0;
            foreach(Drone d in drones)
            {
                ++dataGridView1.RowCount;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount.ToString();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = d.Model;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = d.Operator;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = d.Distance.ToString();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = d.Height.ToString();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = d.Speed.ToString();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = d.Status;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAdd add = new FormAdd();
            add.ShowDialog();
            if (add.created)
            {
                drones.Add(add.Drone);
                RefreshData();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(saveFileDialog1.FileName)))
            {
                foreach (Drone drone in drones)
                {
                    writer.Write(drone.Model);
                    writer.Write(drone.Operator);
                    writer.Write(drone.Distance);
                    writer.Write(drone.Height);
                    writer.Write(drone.Speed);
                    writer.Write(drone.Status);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            drones.Clear();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection cell = dataGridView1.SelectedCells;
            if (cell == null)
            {
                drones.RemoveAt(cell[0].RowIndex);
                RefreshData();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 0;
            drones.Clear();
            saveFileDialog1.FileName = "NewDrones";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName == "NewDrones")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(saveFileDialog1.FileName)))
                {
                    foreach (Drone drone in drones)
                    {
                        writer.Write(drone.Model);
                        writer.Write(drone.Operator);
                        writer.Write(drone.Distance);
                        writer.Write(drone.Height);
                        writer.Write(drone.Speed);
                        writer.Write(drone.Status);
                    }
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(openFileDialog1.FileName)))
            {
                try
                {
                    while (true)
                    {
                        Drone drone = new Drone(reader.ReadString(), reader.ReadString(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadString());
                        drones.Add(drone);
                    }
                }
                catch (EndOfStreamException)
                {

                }
            }
            RefreshData();
        }
    }
}
