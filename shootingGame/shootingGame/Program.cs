using System;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Wave;
using System.IO;

namespace shootinggame
{
    partial class mainProgram : Form
    {
        const int w = 800, h = 600;
        const int buttonN = 4;  // ボタンの数
        const int panelN = 4;  // パネルの数

        // パネル
        private Panel[] panel = new Panel[panelN];
        private const int titleNumber     = 0;
        private const int gameNumber = 1;
        private const int configNumber    = 2;
        private const int listNumber      = 3;

        // 背景画像
        private Image[] backgroundImg = new Image[panelN];

        // ボタン+ボタンname
        private Button[] bt = new Button[buttonN];
        private string[] buttonName = new string[buttonN]
         { "START", "CONFIG", "SOUNDS-LIST-", "EXIT" };

        // NAudio
        private AudioFileReader opening = new AudioFileReader("sample.wav");   // 固定曲
        private LoopStream loop;   // class-LoopStream内の型
        private WaveOut waveOut = new WaveOut();
        private float vol = 0.01f;  // 初期値

        public static void Main()
        {
            Application.Run(new mainProgram());
        }

        public mainProgram()
        {
            double x = 0.5 * (double)w - 50, y = 0.5 * (double)h - 25;

            this.Text = "シューティングゲーム";
            // サイズ固定（最大化とかはできる）
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Width = w;
            this.Height = h;

            for (int i=0; i<panelN; i++) backgroundImg[i] = Image.FromFile(@"..\..\Asset\" + i + ".png");

            // パネル作成
            for (int i = 0; i < panelN; i++)
            {
                panel[i] = new Panel();
                panel[i].Dock = DockStyle.Fill;
                panel[i].Visible = false;
                panel[i].BackgroundImage = backgroundImg[i];
                this.Controls.Add(panel[i]);

                //if(i!=titleNumber) (panel[i] as Control).KeyDown += new KeyEventHandler(backKey);
            }
                

            // ボタン作成
            for (int i = 0; i < buttonN; i++)
            {
                bt[i] = new Button();
                bt[i].Text = buttonName[i];
                bt[i].Width = 100;
                bt[i].Location = new Point((int)x, (int)(y + 2 * i * bt[0].Height));

                bt[i].Click += new EventHandler(bt_Click);

                // ボタン配置
                panel[titleNumber].Controls.Add(bt[i]);
            }

            DrawPage(titleNumber, opening);
        }

        //=======================タイトルバック(ゲーム終了)================================
        [System.Security.Permissions.UIPermission(
            System.Security.Permissions.SecurityAction.Demand,
            Window = System.Security.Permissions.UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            //左キーが押されているか調べる
            if ((keyData & Keys.KeyCode) == Keys.Escape)
            {
                string back_msg = "タイトルへ戻りますか？";
                string end_msg = "ゲームを終了しますか？";
                DialogResult result;
                if (panel[titleNumber].Visible)
                {
                    result = MessageBox.Show(end_msg, "メニュー", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        //timer.Enabled = false;
                        waveOut.Stop();
                        waveOut.Dispose();
                        Application.Exit();
                    }
                }
                else
                {
                    result = MessageBox.Show(back_msg, "メニュー", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        waveOut.Stop();
                        waveOut.Dispose();
                        //if (timer != null) timer.Dispose();
                        DrawPage(titleNumber, opening);
                    }
                }
                //左キーの本来の処理（左側のコントロールにフォーカスを移す）を
                //させたくないときは、trueを返す
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        //======================ボタン================================
        public void bt_Click(Object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "START":
                    DrawPage(gameNumber);
                    this.ActiveControl = panel[gameNumber];
                    game();
                    break;
                case "CONFIG":
                    DrawPage(configNumber);
                    break;
                case "SOUNDS-LIST-":
                    DrawPage(listNumber);
                    break;
                case "EXIT":
                    string msg = "ゲームを終了しますか？";
                    DialogResult result = MessageBox.Show(msg, "終了", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        //timer.Enabled = false;
                        waveOut.Stop();
                        waveOut.Dispose();
                        Application.Exit();
                    }
                    break;
                default: break;
            }
        }

        //=====================描画関数==================================
        public void DrawPage(int pageNumber)
        {
            waveOut.Stop();
            waveOut.Dispose();

            for (int i = 0; i < panelN; i++)
            {
                if (i != pageNumber) panel[i].Visible = false;
                else panel[i].Visible = true;
            }
        }
        public void DrawPage(int pageNumber, AudioFileReader bgmFile)
        {
            loop = new LoopStream(bgmFile);
            play(loop);

            for (int i = 0; i < panelN; i++)
            {
                if (i != pageNumber) panel[i].Visible = false;
                else panel[i].Visible = true;
            }
        }


        public void play(LoopStream loop)
        {
            loop.Position = 0;
            waveOut.Init(loop);
            waveOut.Volume = vol;
            waveOut.Play();
        }
    }

}