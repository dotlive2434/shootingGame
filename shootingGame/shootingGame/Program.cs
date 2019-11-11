using System;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Wave;

class Sample : Form
{
    private AudioFileReader reader =
              new AudioFileReader("sample.wav");
    private WaveOut waveOut = new WaveOut();
    private Button[] bt = new Button[3];
    private TrackBar tb;
    private string[] str = new string[3] { "|＞", "||", "■" };
    private float vol;
    private long wavePosition=0;

    public static void Main()
    {
        Application.Run(new Sample());
    }

    public Sample()
    {
        this.Text = "サンプル";
        this.Width = 300; this.Height = 150;

        tb = new TrackBar();
        tb.TickStyle = TickStyle.Both;
        tb.Maximum = 100;
        // 初期値を設定
        tb.Value = 0;
        vol = tb.Value;
        // 描画される目盛りの刻みを設定
        tb.TickFrequency = 10;
        tb.Dock = DockStyle.Bottom;
        tb.Parent = this;

        for (int i = 0; i < bt.Length; i++)
        {
            bt[i] = new Button();
            bt[i].Text = str[i];
            bt[i].Width = 50;
            bt[i].Location = new Point(20 + i * this.Width/3, 20);

            bt[i].Parent = this;

            bt[i].Click += new EventHandler(bt_Click);
        }
        waveOut.Init(reader);
        // 値が変更された際のイベントハンドらーを追加
        tb.ValueChanged += new EventHandler(tb_ValueChanged);
    }

    public void bt_Click(Object sender, EventArgs e)
    {
        if (sender == bt[0])
        {
            reader.Position = wavePosition;
            waveOut.Volume = vol;
            waveOut.Play();
        }
        else if (sender == bt[1])
        {
            wavePosition = reader.Position;
            waveOut.Stop();
        }
        else if (sender == bt[2])
        {
            wavePosition = 0;
            waveOut.Stop();
        }
    }

    public void tb_ValueChanged(Object sender, EventArgs e)
    {
        vol = tb.Value / 100f;
        waveOut.Volume = vol;
    }
}