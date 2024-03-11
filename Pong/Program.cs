using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Pong{
    class Game : GameWindow
    {
        private int textTextureId;

        int xDaBola = 0;
        int yDaBola = 0;
        int velocidade = 0;
        int velocidadeBolaEmX = 3;
        int velocidadeBolaEmY = 3;

        int yJogador1 = 0;
        int yJogador2 = 0;

        int xJogador1()
        {
            return - ClientSize.Width / 2 + 7;
        }
        int xJogador2()
        {
            return ClientSize.Width / 2 - 7;
        }

        int pontosJogador1 = 0;
        int pontosJogador2 = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color4.Black);

            // Inicializa o OpenGL para desenhar texto
            textTextureId = TextRenderer.LoadTexture();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (pontosJogador1 > 9 || pontosJogador2 > 9)
            {
                string mensagemVencedor = pontosJogador1 > pontosJogador2 ? "Jogador 1 venceu!" : "Jogador 2 venceu!";

                // Calcula a posição do texto no centro da tela
                float textoX = ClientSize.Width / 2 - mensagemVencedor.Length * 6; // Ajuste o espaçamento conforme necessário
                float textoY = ClientSize.Height / 2;
        
                // Cor do texto
                Color4 textColor = Color4.White;
        
                // Renderiza o texto na tela
                TextRenderer.DrawText(mensagemVencedor, (int)textoX, (int)textoY, textColor, textTextureId);
            }
            // Direção da bola
            if((xDaBola + 14 >= xJogador2() && yDaBola - 7 <= yJogador2 + 25 && yDaBola + 7 >= yJogador2 - 25) || (xDaBola - 14 <= xJogador1() && yDaBola - 7 <= yJogador1 + 25 && yDaBola + 7 >= yJogador1 - 25))
            {
                velocidadeBolaEmX = - velocidadeBolaEmX;
            } 
                else if (xDaBola + 7 >= xJogador2())
                {
                    pontosJogador1++;
                    xDaBola = 0;
                    yDaBola = 0;
                    velocidade++;
                    if (velocidade%5 == 0)
                    {
                        velocidadeBolaEmX = velocidade;
                        velocidadeBolaEmY = -velocidade;
                    }
                }
                else if (xDaBola - 7 <= xJogador1())
                {
                    pontosJogador2++;
                    xDaBola = 0;
                    yDaBola = 0;
                    velocidade++;
                    if (velocidade%5 == 0)
                    {
                        velocidadeBolaEmX = - velocidade;
                        velocidadeBolaEmY = velocidade;
                    }
                }

            if(yDaBola + 7 > ClientSize.Height / 2 || (yDaBola - 7) * (-1) > ClientSize.Height / 2)
            {
                velocidadeBolaEmY = - velocidadeBolaEmY;
            }

            // Movimentação dos jogadores
            if(Keyboard.GetState().IsKeyDown(Key.W) && yJogador1 + 25 <= ClientSize.Height / 2)
            {
                yJogador1 = yJogador1 + 5;
            }

            if(Keyboard.GetState().IsKeyDown(Key.S) && yJogador1 - 25 >= - ClientSize.Height / 2)
            {
                yJogador1 = yJogador1 - 5;
            }

            if(Keyboard.GetState().IsKeyDown(Key.Up) && yJogador2 + 25 <= ClientSize.Height / 2)
            {
                yJogador2 = yJogador2 + 5;
            }

            if(Keyboard.GetState().IsKeyDown(Key.Down) && yJogador2 - 25 >= - ClientSize.Height / 2)
            {
                yJogador2 = yJogador2 - 5;
            }

            // Movimento da bola
            xDaBola = xDaBola + velocidadeBolaEmX;
            yDaBola = yDaBola + velocidadeBolaEmY;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            Matrix4 projection = Matrix4.CreateOrthographic(ClientSize.Width, ClientSize.Height, 0.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            DesenharRetangulo(xDaBola, yDaBola, 14, 14);
            DesenharRetangulo(xJogador1(), yJogador1, 14, 50);
            DesenharRetangulo(xJogador2(), yJogador2, 14, 50);
            DesenharRetangulo(0, 0, 1, ClientSize.Height);
            DesenharPlacar(- ClientSize.Width / 4, ClientSize.Height / 4, pontosJogador1);
            DesenharPlacar(ClientSize.Width / 4, ClientSize.Height / 4, pontosJogador2);

            SwapBuffers();
        }

        void DesenharRetangulo(int x, int y, int largura, int altura)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f * largura + x, -0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x, -0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x, 0.5f * altura + y);
            GL.Vertex2(-0.5f * largura + x, 0.5f * altura + y);
            GL.End();
        }

        void DesenharPlacar(int x, int y, int pontos)
        {
            Color4 textColor = Color4.White;
            TextRenderer.DrawText(pontos.ToString(), x, y, textColor, textTextureId, ClientSize.Width, ClientSize.Height);
        }

    }
    class Program
    {
        static void Main()
        {
            using (var game = new Game())
            {
            game.Run();
            }
        }
    }
    static class TextRenderer
    {
        public static int LoadTexture()
        {
            int texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            // Atributos de textura
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return texId;
        }

        public static void DrawText(string text, int x, int y, Color4 color, int textureId, int largura, int altura)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.Color4(color);

            foreach (char c in text)
            {
                DrawCharacter(c, x, y);
                x += 12; // Ajuste o espaçamento entre caracteres conforme necessário
            }
        }

        private static void DrawCharacter(char c, int x, int y)
        {

            switch (c)
            {
                case '0':
                    DrawSegment(x, y, 1, 0, 1, 7);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    DrawSegment(x, y, 4, 7, 4, 0);
                    DrawSegment(x, y, 4, 0, 1, 0);
                    DrawSegment(x, y, 1, 0, 4, 7);
                    break;
                case '1':
                    DrawSegment(x, y, 4, 7, 4, 0);
                    break;
                case '2':
                    DrawSegment(x, y, 1, 0, 4, 0);
                    DrawSegment(x, y, 1, 0, 1, 3.5f);
                    DrawSegment(x, y, 4, 3.5f, 1, 3.5f);
                    DrawSegment(x, y, 4, 3.5f, 4, 7);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    break;
                case '3':
                    DrawSegment(x, y, 1, 0, 4, 0);
                    DrawSegment(x, y, 4, 0, 4, 7);
                    DrawSegment(x, y, 1, 3.5f, 4, 3.5f);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    break;
                case '4':
                    DrawSegment(x, y, 4, 0, 4, 7);
                    DrawSegment(x, y, 1, 3.5f, 4, 3.5f);
                    DrawSegment(x, y, 1, 3.5f, 4, 7);
                    break;
                case '5':
                    DrawSegment(x, y, 1, 0, 4, 0);
                    DrawSegment(x, y, 4, 0, 4, 3.5f);
                    DrawSegment(x, y, 1, 3.5f, 4, 3.5f);
                    DrawSegment(x, y, 1, 3.5f, 1, 7);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    break;
                case '6':
                    DrawSegment(x, y, 4, 0, 1, 0);
                    DrawSegment(x, y, 1, 0, 1, 7);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    DrawSegment(x, y, 4, 3.5f, 1, 3.5f);
                    DrawSegment(x, y, 4, 3.5f, 4, 0);
                    break;
                case '7':
                    DrawSegment(x, y, 1, 7, 4, 7);
                    DrawSegment(x, y, 4, 0, 4, 7);
                    break;
                case '8':
                    DrawSegment(x, y, 1, 0, 1, 7);
                    DrawSegment(x, y, 1, 7, 4, 7);
                    DrawSegment(x, y, 4, 7, 4, 0);
                    DrawSegment(x, y, 4, 0, 1, 0);
                    DrawSegment(x, y, 1, 3.5f, 4, 3.5f);
                    break;
                case '9':
                    DrawSegment(x, y, 1, 0, 4, 0);
                    DrawSegment(x, y, 4, 0, 4, 7);
                    DrawSegment(x, y, 4, 7, 1, 7);
                    DrawSegment(x, y, 1, 7, 1, 3.5f);
                    DrawSegment(x, y, 1, 3.5f, 4, 3.5f);
                    break;
                default:
                    break;
            }
        }

        private static void DrawSegment(int x, int y, float x1, float y1, float x2, float y2)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(x + x1 * 12, y + y1 * 16);
            GL.Vertex2(x + x2 * 12, y + y2 * 16);
            GL.End();
        }
    }
}
