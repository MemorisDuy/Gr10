using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace LuckyWheelClient
{
    public class FormChinh : Form
    {
        private readonly string tenDangNhap;
        private Label lblChaoMung;
        private Label lblDiem;
        private Label lblLuot;
        private Label lblThoiGian;
        private Button btnVaoVongQuay;
        private Button btnLichSu;
        private Button btnVIP;
        private Button btnDangKyVIP;
        private Button btnDangKy;
        private Timer updateTimer;
        private Label lblTrangThaiServer;

        // Biến cho trắc nghiệm
        private TabControl tabControl;
        private TabPage tabMain;
        private TabPage tabQuiz;
        private Panel pnlQuiz;
        private Label lblQuizQuestion;
        private RadioButton[] radQuizOptions;
        private Button btnQuizSubmit;
        private Label lblQuizPoints;
        private PictureBox picVietnamFlag;
        private Panel pnlReward;
        private Label lblRewardTitle;
        private Label lblRewardIcon;
        private Label lblRewardDescription;
        private int quizPoints = 0;
        private int currentQuizQuestion = 0;
        private List<QuizQuestion> quizQuestions;
        private Random randomReward;

        private class QuizQuestion
        {
            public string Question { get; set; }
            public string[] Options { get; set; }
            public int CorrectAnswer { get; set; }
        }

        public FormChinh(string tenDangNhap)
        {
            this.tenDangNhap = tenDangNhap;
            this.Text = "🎮 Vòng Quay May Mắn - Trang Chính";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 250);
            this.MinimumSize = new Size(700, 500); // Đặt kích thước tối thiểu

            randomReward = new Random();
            InitializeComponents();
            InitializeQuiz();

            updateTimer = new Timer
            {
                Interval = 1000
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            this.Resize += FormChinh_Resize;
            this.Load += FormChinh_Load;

            RefreshThongTin();
            KiemTraVaHienThiNutVIP();
            CheckServerConnection();
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {
            // Điều chỉnh vị trí các thành phần khi form tải xong
            AdjustControlPositions();
        }

        private void FormChinh_Resize(object sender, EventArgs e)
        {
            // Điều chỉnh vị trí các thành phần khi form được thay đổi kích thước
            AdjustControlPositions();
        }

        private void AdjustControlPositions()
        {
            // Điều chỉnh vị trí các panel trong tab chính
            if (tabMain != null && tabMain.Controls.Count > 0)
            {
                Panel pnlInfo = tabMain.Controls.OfType<Panel>().FirstOrDefault();
                FlowLayoutPanel pnlButtons = tabMain.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

                if (pnlInfo != null && pnlButtons != null)
                {
                    pnlInfo.Location = new Point((tabMain.ClientSize.Width - pnlInfo.Width) / 2,
                                              (tabMain.ClientSize.Height - pnlInfo.Height - pnlButtons.Height - 20) / 2);
                    pnlButtons.Location = new Point((tabMain.ClientSize.Width - pnlButtons.Width) / 2,
                                                 pnlInfo.Bottom + 20);
                }
            }

            // Điều chỉnh vị trí panel trắc nghiệm trong tab Quiz
            if (tabQuiz != null && pnlQuiz != null)
            {
                pnlQuiz.Location = new Point((tabQuiz.ClientSize.Width - pnlQuiz.Width) / 2,
                                          (tabQuiz.ClientSize.Height - pnlQuiz.Height) / 2);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            updateTimer?.Stop();
            updateTimer?.Dispose();
        }

        private void InitializeComponents()
        {
            // Tạo TabControl với dock fill để tự động điền đầy form
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(10, 3), // Tạo padding để không sát các cạnh
                Font = new Font("Segoe UI", 9),
                Margin = new Padding(10)
            };

            // Tab chính
            tabMain = new TabPage
            {
                Text = "🏠 Trang Chính"
            };

            // Tab trắc nghiệm
            tabQuiz = new TabPage
            {
                Text = "📚 Trắc Nghiệm"
            };

            tabControl.TabPages.Add(tabMain);
            tabControl.TabPages.Add(tabQuiz);
            this.Controls.Add(tabControl);

            // Panel thông tin người dùng (sẽ căn giữa động)
            Panel pnlInfo = new Panel
            {
                Size = new Size(500, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.None
            };

            lblChaoMung = new Label
            {
                Text = $"🎉 Chào mừng, {tenDangNhap}!",
                AutoSize = false,
                Size = new Size(500, 30),
                Location = new Point(0, 20),
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 150),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblDiem = new Label
            {
                Text = "Điểm: ...",
                AutoSize = false,
                Size = new Size(500, 20),
                Location = new Point(0, 50),
                Font = new Font("Arial", 12),
                ForeColor = Color.FromArgb(50, 120, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblLuot = new Label
            {
                Text = "Lượt quay miễn phí: ...",
                AutoSize = false,
                Size = new Size(500, 20),
                Location = new Point(0, 80),
                Font = new Font("Arial", 12),
                ForeColor = Color.FromArgb(50, 120, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblThoiGian = new Label
            {
                Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                AutoSize = false,
                Size = new Size(500, 20),
                Location = new Point(0, 110),
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(70, 70, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblTrangThaiServer = new Label
            {
                Text = "Đang kiểm tra kết nối...",
                AutoSize = true,
                Location = new Point(pnlInfo.Width - 150 - 20, 20),
                Font = new Font("Arial", 8),
                ForeColor = Color.Gray
            };

            pnlInfo.Controls.Add(lblChaoMung);
            pnlInfo.Controls.Add(lblDiem);
            pnlInfo.Controls.Add(lblLuot);
            pnlInfo.Controls.Add(lblThoiGian);
            pnlInfo.Controls.Add(lblTrangThaiServer);

            // Panel chứa các nút chức năng (căn giữa)
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Size = new Size(300, 300),
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.None
            };

            btnVaoVongQuay = CreateStyledButton("👉 Vào Vòng Quay", Color.FromArgb(52, 152, 219));
            btnLichSu = CreateStyledButton("📜 Xem Lịch Sử Quay", Color.FromArgb(155, 89, 182));
            btnVIP = CreateStyledButton("💎 Vòng Quay VIP", Color.FromArgb(241, 196, 15));
            btnDangKyVIP = CreateStyledButton("🔑 Đăng ký VIP (500 điểm)", Color.FromArgb(230, 126, 34));
            btnDangKy = CreateStyledButton("📝 Đăng Ký Tài Khoản", Color.FromArgb(231, 76, 60));

            btnVaoVongQuay.Click += BtnVaoVongQuay_Click;
            btnLichSu.Click += BtnLichSu_Click;
            btnVIP.Click += BtnVIP_Click;
            btnDangKyVIP.Click += BtnDangKyVIP_Click;
            btnDangKy.Click += BtnDangKy_Click;

            pnlButtons.Controls.Add(btnVaoVongQuay);
            pnlButtons.Controls.Add(btnLichSu);
            pnlButtons.Controls.Add(btnVIP);
            pnlButtons.Controls.Add(btnDangKyVIP);
            pnlButtons.Controls.Add(btnDangKy);

            tabMain.Controls.Add(pnlInfo);
            tabMain.Controls.Add(pnlButtons);
        }

        private void InitializeQuiz()
        {
            // Panel trắc nghiệm (căn giữa trong tabQuiz)
            pnlQuiz = new Panel
            {
                Size = new Size(600, 400),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.None
            };

            lblQuizPoints = new Label
            {
                Text = "Điểm trắc nghiệm: 0",
                AutoSize = true,
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblQuizQuestion = new Label
            {
                Location = new Point(20, 50),
                Size = new Size(560, 40),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter
            };

            radQuizOptions = new RadioButton[4];
            for (int i = 0; i < 4; i++)
            {
                radQuizOptions[i] = new RadioButton
                {
                    Location = new Point(40, 100 + i * 40),
                    Size = new Size(520, 35),
                    Font = new Font("Segoe UI", 8),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlQuiz.Controls.Add(radQuizOptions[i]);
            }

            btnQuizSubmit = new Button
            {
                Text = "Xác nhận",
                Location = new Point((600 - 120) / 2, 350), // Căn giữa button
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8),
                Cursor = Cursors.Hand
            };
            btnQuizSubmit.FlatAppearance.BorderSize = 0;
            btnQuizSubmit.Click += BtnQuizSubmit_Click;

            // Lá cờ Việt Nam (hiển thị khi trả lời đúng)
            picVietnamFlag = new PictureBox
            {
                Location = new Point(600 - 100 - 20, 20),
                Size = new Size(100, 60),
                Visible = false
            };
            Bitmap flag = new Bitmap(100, 60);
            using (Graphics g = Graphics.FromImage(flag))
            {
                g.Clear(Color.Red);
                g.FillRectangle(Brushes.Yellow, 35, 15, 30, 30);
                g.FillPolygon(Brushes.Yellow, new Point[]
                {
                    new Point(50, 15),
                    new Point(55, 25),
                    new Point(65, 25),
                    new Point(57, 30),
                    new Point(60, 40),
                    new Point(50, 35),
                    new Point(40, 40),
                    new Point(43, 30),
                    new Point(35, 25),
                    new Point(45, 25)
                });
            }
            picVietnamFlag.Image = flag;

            // Panel phần thưởng (căn giữa trong pnlQuiz)
            pnlReward = new Panel
            {
                Size = new Size(560, 300),
                BackColor = Color.FromArgb(255, 245, 230),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            // Điều chỉnh vị trí của panel phần thưởng để căn giữa trong pnlQuiz
            pnlReward.Location = new Point((600 - 560) / 2, (400 - 300) / 2);

            lblRewardTitle = new Label
            {
                Text = "🎉 CHÚC MỪNG BẠN NHẬN ĐƯỢC 🎉",
                Location = new Point(0, 20),
                Size = new Size(560, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 69, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblRewardIcon = new Label
            {
                Location = new Point(0, 60),
                Size = new Size(560, 80),
                Font = new Font("Segoe UI", 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblRewardDescription = new Label
            {
                Location = new Point(0, 150),
                Size = new Size(560, 60),
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.FromArgb(0, 128, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlReward.Controls.AddRange(new Control[] { lblRewardTitle, lblRewardIcon, lblRewardDescription });

            pnlQuiz.Controls.AddRange(new Control[] { lblQuizPoints, lblQuizQuestion, btnQuizSubmit, picVietnamFlag, pnlReward });
            tabQuiz.Controls.Add(pnlQuiz);

            // Danh sách 10 câu hỏi về ngày 30/4
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Ngày 30/4/1975 được gọi là sự kiện gì tại Việt Nam?",
                    Options = new[] { "Ngày Giải phóng miền Nam", "Ngày Quốc khánh", "Ngày Thống nhất đất nước", "Cả A và C đều đúng" },
                    CorrectAnswer = 3
                },
                new QuizQuestion
                {
                    Question = "Ai là Tổng thống cuối cùng của chính quyền Sài Gòn đầu hàng ngày 30/4/1975?",
                    Options = new[] { "Ngô Đình Diệm", "Nguyễn Văn Thiệu", "Dương Văn Minh", "Trần Văn Hương" },
                    CorrectAnswer = 2
                },
                new QuizQuestion
                {
                    Question = "Chiến dịch nào dẫn đến chiến thắng ngày 30/4/1975?",
                    Options = new[] { "Chiến dịch Điện Biên Phủ", "Chiến dịch Hồ Chí Minh", "Chiến dịch Tây Nguyên", "Chiến dịch Huế-Đà Nẵng" },
                    CorrectAnswer = 1
                },
                new QuizQuestion
                {
                    Question = "Địa điểm nào là nơi diễn ra lễ đầu hàng chính thức vào ngày 30/4/1975?",
                    Options = new[] { "Dinh Độc Lập", "Tòa Thị chính Sài Gòn", "Sân bay Tân Sơn Nhất", "Nhà Quốc hội" },
                    CorrectAnswer = 0
                },
                new QuizQuestion
                {
                    Question = "Cờ giải phóng được cắm ở đâu vào trưa ngày 30/4/1975?",
                    Options = new[] { "Sân bay Tân Sơn Nhất", "Dinh Độc Lập", "Cầu Sài Gòn", "Chợ Bến Thành" },
                    CorrectAnswer = 1
                },
                new QuizQuestion
                {
                    Question = "Ai là người chỉ huy Chiến dịch Hồ Chí Minh năm 1975?",
                    Options = new[] { "Võ Nguyên Giáp", "Văn Tiến Dũng", "Lê Đức Thọ", "Phạm Văn Đồng" },
                    CorrectAnswer = 1
                },
                new QuizQuestion
                {
                    Question = "Sự kiện 30/4/1975 đánh dấu sự kết thúc của chiến tranh nào?",
                    Options = new[] { "Chiến tranh Đông Dương", "Chiến tranh Việt Nam", "Chiến tranh lạnh", "Chiến tranh thế giới thứ hai" },
                    CorrectAnswer = 1
                },
                new QuizQuestion
                {
                    Question = "Xe tăng nào đã húc đổ cổng Dinh Độc Lập ngày 30/4/1975?",
                    Options = new[] { "Xe tăng số 390", "Xe tăng số 843", "Xe tăng số 390 và 843", "Xe tăng số 720" },
                    CorrectAnswer = 2
                },
                new QuizQuestion
                {
                    Question = "Tên gọi chính thức của chính quyền Sài Gòn trước ngày 30/4/1975 là gì?",
                    Options = new[] { "Việt Nam Dân chủ Cộng hòa", "Cộng hòa miền Nam Việt Nam", "Việt Nam Cộng hòa", "Nhà nước Việt Nam" },
                    CorrectAnswer = 2
                },
                new QuizQuestion
                {
                    Question = "Vào ngày 30/4/1975, ai đã phát biểu 'Toàn thắng về ta' trên đài phát thanh?",
                    Options = new[] { "Hồ Chí Minh", "Dương Văn Minh", "Võ Nguyên Giáp", "Không ai phát biểu câu này" },
                    CorrectAnswer = 3
                }
            };

            DisplayQuizQuestion();
        }

        private void DisplayQuizQuestion()
        {
            if (currentQuizQuestion >= quizQuestions.Count)
            {
                lblQuizQuestion.Text = $"🎉 Hoàn thành! Điểm: {quizPoints}";
                foreach (var rad in radQuizOptions)
                {
                    rad.Visible = false;
                }
                btnQuizSubmit.Visible = false;
                ShowReward();
                return;
            }

            QuizQuestion question = quizQuestions[currentQuizQuestion];
            lblQuizQuestion.Text = $"Câu {currentQuizQuestion + 1}: {question.Question}";
            for (int i = 0; i < 4; i++)
            {
                radQuizOptions[i].Text = question.Options[i];
                radQuizOptions[i].Checked = false;
                radQuizOptions[i].Visible = true;
            }
            btnQuizSubmit.Visible = true;
            btnQuizSubmit.Enabled = true;
            picVietnamFlag.Visible = false;
            pnlReward.Visible = false;
        }

        private void ShowReward()
        {
            string[] rewards = { "Ô tô hạng sang", "Xe máy SH", "Tiền mặt 100 triệu" };
            string[] rewardIcons = { "🚗💨", "🏍️🔥", "💰🤑" };
            string[] rewardDescriptions =
            {
                "Wow, bạn trúng một chiếc ô tô hạng sang! 🎉 Lái xe vi vu nào! 😎",
                "Chúc mừng! Bạn nhận được xe máy SH siêu xịn! 🏍️ Đi phượt thôi! 🔥",
                "Tiền về như nước! 💸 Bạn nhận được 100 triệu tiền mặt! 🤑"
            };

            int rewardIndex = randomReward.Next(rewards.Length);
            lblRewardTitle.Text = "🎉 CHÚC MỪNG BẠN NHẬN ĐƯỢC 🎉";
            lblRewardIcon.Text = rewardIcons[rewardIndex];
            lblRewardDescription.Text = rewardDescriptions[rewardIndex];
            pnlReward.Visible = true;

            // Đảm bảo panel phần thưởng nằm giữa
            pnlReward.Location = new Point((pnlQuiz.Width - pnlReward.Width) / 2, (pnlQuiz.Height - pnlReward.Height) / 2);
        }

        private void BtnQuizSubmit_Click(object sender, EventArgs e)
        {
            btnQuizSubmit.Enabled = false;
            int selectedOption = -1;
            for (int i = 0; i < 4; i++)
            {
                if (radQuizOptions[i].Checked)
                {
                    selectedOption = i;
                    break;
                }
            }

            if (selectedOption == -1)
            {
                MessageBox.Show("Vui lòng chọn một đáp án!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnQuizSubmit.Enabled = true;
                return;
            }

            QuizQuestion question = quizQuestions[currentQuizQuestion];
            if (selectedOption == question.CorrectAnswer)
            {
                quizPoints += 10;
                lblQuizPoints.Text = $"Điểm trắc nghiệm: {quizPoints}";
                MessageBox.Show("Đáp án đúng! +10 điểm", "Chúc mừng");
                picVietnamFlag.Visible = true;
            }
            else
            {
                MessageBox.Show($"Đáp án sai! Đáp án đúng là: {question.Options[question.CorrectAnswer]}", "Thông báo");
            }

            currentQuizQuestion++;
            DisplayQuizQuestion();
        }

        private Button CreateStyledButton(string text, Color color)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(250, 40),
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private async void CheckServerConnection()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var connectTask = client.BeginConnect("localhost", 9876, null, null);
                    bool connected = connectTask.AsyncWaitHandle.WaitOne(2000);

                    if (connected)
                    {
                        lblTrangThaiServer.Text = "✅ Kết nối server thành công";
                        lblTrangThaiServer.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblTrangThaiServer.Text = "⚠️ Đang chạy ở chế độ ngoại tuyến";
                        lblTrangThaiServer.ForeColor = Color.Orange;
                    }
                }
            }
            catch
            {
                lblTrangThaiServer.Text = "⚠️ Đang chạy ở chế độ ngoại tuyến";
                lblTrangThaiServer.ForeColor = Color.Orange;
            }
        }

        private void BtnVaoVongQuay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FormQuayVongGUI quayForm = new FormQuayVongGUI(tenDangNhap)
            {
                Owner = this
            };
            quayForm.ShowDialog();
            RefreshThongTin();
        }

        private void BtnLichSu_Click(object sender, EventArgs e)
        {
            FormLichSuQuay lichSu = new FormLichSuQuay(tenDangNhap);
            lichSu.ShowDialog();
        }

        private void BtnVIP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FormQuayVongVIP vipForm = new FormQuayVongVIP(tenDangNhap)
            {
                Owner = this
            };
            vipForm.ShowDialog();
            RefreshThongTin();
        }

        private async void BtnDangKyVIP_Click(object sender, EventArgs e)
        {
            DialogResult xacNhan = MessageBox.Show("Bạn muốn nâng cấp tài khoản VIP với 500 điểm?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan == DialogResult.Yes)
            {
                try
                {
                    btnDangKyVIP.Enabled = false;
                    btnDangKyVIP.Text = "Đang xử lý...";

                    bool upgradeSuccess = false;

                    using (TcpClient client = new TcpClient())
                    {
                        var connectTask = client.BeginConnect("localhost", 9876, null, null);
                        bool connected = connectTask.AsyncWaitHandle.WaitOne(3000);

                        if (connected)
                        {
                            client.EndConnect(connectTask);
                            using (NetworkStream stream = client.GetStream())
                            {
                                string request = $"UPGRADEVIP|{tenDangNhap}";
                                byte[] data = Encoding.UTF8.GetBytes(request);
                                stream.Write(data, 0, data.Length);

                                byte[] buffer = new byte[1024];
                                int byteCount = stream.Read(buffer, 0, buffer.Length);
                                string response = Encoding.UTF8.GetString(buffer, 0, byteCount);

                                upgradeSuccess = response == "VIP_OK";
                            }
                        }
                        else
                        {
                            upgradeSuccess = LocalAuthManager.UpgradeToVIP(tenDangNhap);
                        }
                    }

                    if (upgradeSuccess)
                    {
                        MessageBox.Show("🎉 Chúc mừng! Tài khoản đã được nâng cấp VIP.");
                        RefreshThongTin();
                        KiemTraVaHienThiNutVIP();
                    }
                    else
                    {
                        MessageBox.Show("❌ Không thể nâng cấp VIP: Không đủ điểm hoặc đã là VIP.");
                    }
                }
                catch (Exception)
                {
                    bool upgradeSuccess = LocalAuthManager.UpgradeToVIP(tenDangNhap);
                    if (upgradeSuccess)
                    {
                        MessageBox.Show("🎉 Chúc mừng! Tài khoản đã được nâng cấp VIP.");
                    }
                    else
                    {
                        MessageBox.Show("❌ Không thể nâng cấp VIP: Không đủ điểm hoặc đã là VIP.");
                    }
                    RefreshThongTin();
                    KiemTraVaHienThiNutVIP();
                }
                finally
                {
                    btnDangKyVIP.Enabled = true;
                    btnDangKyVIP.Text = "🔑 Đăng ký VIP (500 điểm)";
                }
            }
        }

        private void BtnDangKy_Click(object sender, EventArgs e)
        {
            FormDangKy formDangKy = new FormDangKy();
            formDangKy.ShowDialog();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            lblThoiGian.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (DateTime.Now.Second == 0)
            {
                RefreshThongTin();
                KiemTraVaHienThiNutVIP();
            }
        }

        public async void RefreshThongTin()
        {
            try
            {
                int points = 1000;
                int freeTurns = 5;

                using (TcpClient client = new TcpClient())
                {
                    var connectTask = client.BeginConnect("localhost", 9876, null, null);
                    bool connected = connectTask.AsyncWaitHandle.WaitOne(3000);

                    if (connected)
                    {
                        client.EndConnect(connectTask);
                        using (NetworkStream stream = client.GetStream())
                        {
                            string request = $"INFO|{tenDangNhap}";
                            byte[] data = Encoding.UTF8.GetBytes(request);
                            stream.Write(data, 0, data.Length);

                            byte[] buffer = new byte[1024];
                            int byteCount = stream.Read(buffer, 0, buffer.Length);
                            string response = Encoding.UTF8.GetString(buffer, 0, byteCount);

                            if (response.StartsWith("INFO|"))
                            {
                                string[] parts = response.Split('|');
                                if (parts.Length >= 3)
                                {
                                    points = int.Parse(parts[1]);
                                    freeTurns = int.Parse(parts[2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        points = LocalAuthManager.GetUserPoints(tenDangNhap);
                    }
                }

                lblDiem.Text = $"Điểm: {points}";
                lblLuot.Text = $"Lượt quay miễn phí: {freeTurns}";
                lblChaoMung.Text = $"🎉 Chào mừng, {tenDangNhap}!";

                // Căn chỉnh vị trí của các label trong panel để đảm bảo chúng ở giữa
                CenterLabelsInPanel();
            }
            catch (Exception)
            {
                int points = LocalAuthManager.GetUserPoints(tenDangNhap);
                lblDiem.Text = $"Điểm: {points}";
                lblLuot.Text = $"Lượt quay miễn phí: 5";
                lblChaoMung.Text = $"🎉 Chào mừng, {tenDangNhap}!";

                // Căn chỉnh vị trí của các label trong panel để đảm bảo chúng ở giữa
                CenterLabelsInPanel();
            }
        }

        private void CenterLabelsInPanel()
        {
            // Tìm panel chứa thông tin
            Panel pnlInfo = tabMain.Controls.OfType<Panel>().FirstOrDefault();
            if (pnlInfo != null)
            {
                // Điều chỉnh lại vị trí các label trong panel
                lblChaoMung.Width = pnlInfo.Width;
                lblDiem.Width = pnlInfo.Width;
                lblLuot.Width = pnlInfo.Width;
                lblThoiGian.Width = pnlInfo.Width;

                lblChaoMung.Location = new Point(0, lblChaoMung.Location.Y);
                lblDiem.Location = new Point(0, lblDiem.Location.Y);
                lblLuot.Location = new Point(0, lblLuot.Location.Y);
                lblThoiGian.Location = new Point(0, lblThoiGian.Location.Y);
            }
        }

        private async void KiemTraVaHienThiNutVIP()
        {
            try
            {
                bool isVIP = false;

                using (TcpClient client = new TcpClient())
                {
                    var connectTask = client.BeginConnect("localhost", 9876, null, null);
                    bool connected = connectTask.AsyncWaitHandle.WaitOne(3000);

                    if (connected)
                    {
                        client.EndConnect(connectTask);
                        using (NetworkStream stream = client.GetStream())
                        {
                            string request = $"CHECKVIP|{tenDangNhap}";
                            byte[] data = Encoding.UTF8.GetBytes(request);
                            stream.Write(data, 0, data.Length);

                            byte[] buffer = new byte[1024];
                            int byteCount = stream.Read(buffer, 0, buffer.Length);
                            string response = Encoding.UTF8.GetString(buffer, 0, byteCount);

                            isVIP = response == "VIP|TRUE";
                        }
                    }
                    else
                    {
                        isVIP = LocalAuthManager.IsUserVIP(tenDangNhap);
                    }
                }

                btnVIP.Visible = isVIP;
                btnDangKyVIP.Visible = !isVIP;

                if (btnVIP.Visible)
                {
                    btnVIP.BackColor = Color.Gold;
                    btnVIP.ForeColor = Color.DarkBlue;
                }
            }
            catch (Exception)
            {
                bool isVIP = LocalAuthManager.IsUserVIP(tenDangNhap);
                btnVIP.Visible = isVIP;
                btnDangKyVIP.Visible = !isVIP;

                if (btnVIP.Visible)
                {
                    btnVIP.BackColor = Color.Gold;
                    btnVIP.ForeColor = Color.DarkBlue;
                }
            }
        }
    }
}