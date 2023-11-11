using System.Text.RegularExpressions;

string? MontarMenu()
{
    Console.Clear();
    Console.WriteLine("---- Bem-vindo ao Jogo da Forca ----");
    Console.WriteLine();
    Console.WriteLine("1 - Começar jogo");
    Console.WriteLine("0 - Sair");

    return Console.ReadLine();
}

void ComecarJogo()
{
    var escolha = MontarMenu();

    while (escolha != "0")
    {
        if (escolha == "1")
        {
            IniciarJogo();
        }

        escolha = MontarMenu();
    }
}

//escolher uma palavra dentro de uma lista de palavras
void IniciarJogo()
{
    List<string> listaDePalavras = new List<string>()
    {
        "Teste","Carro","Cachorro","Gabinete","Carne","Onomatopeia","Paralelepipedo","Chuva","Corredor",
        "Otorrinolaringologista","Televisao","Espada","Telefone","Escada","Capa","Monitor","Microfone",
        "Fogao","Copo","Controle","Biscuit","Biscoito","Cama","Casal","Cabo","Passarinho","Jogo","Focinho",
        "Gato","Grama","Pedra","Arvore","Mesa","Elefante","Macaco","Girafa","Rinoceronte","Sapato","Sapo",
        "Boneco","Receptor","Laranja","Pera","Maca","Flecha","Arco","Pente","Minhoca","Coruja","Moto","Bicicleta",
        "Aviao","Aeromoca","Aeroporto"
    };

    var random = new Random();
    var indicePalavraEscolhida = random.Next(1, listaDePalavras.Count) - 1;
    var palavraEscolhida = listaDePalavras[indicePalavraEscolhida];
    var vidas = 7;
    var letrasTentadas = new HashSet<char>();
    var mensagemErro = string.Empty;
    var fimDeJogo = false;
    var chute = string.Empty;
    var acertouChute = false;

    while (fimDeJogo == false)
    {
        chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, mensagemErro);

        mensagemErro = ValidarChute(chute, letrasTentadas.ToList());

        if (mensagemErro != string.Empty)
            continue;

        letrasTentadas.Add(char.ToLower(chute[0]));

        acertouChute = VerificarChute(palavraEscolhida, chute);

        if (acertouChute)
        {
            fimDeJogo = VerificarFimDeJogo(vidas, letrasTentadas.ToList(), palavraEscolhida);

            if (fimDeJogo)
            {
                MontarTelaVitoria(vidas, palavraEscolhida);
                continue;
            }
        }

        if (!acertouChute)
        {
            vidas--;

            fimDeJogo = VerificarFimDeJogo(vidas, letrasTentadas.ToList(), palavraEscolhida);

            if (fimDeJogo)
            {
                MontarTelaDerrota(palavraEscolhida, letrasTentadas);
            }
        }
    }

}

string? MontarTelaJogo(string palavra, int vidas, HashSet<char> letrasTentadas, string mensagemErro)
{
    Console.Clear();
    Console.WriteLine(" - - - Jogo da Forca - - - ");
    Console.WriteLine();
    Console.WriteLine(MontarPalavraTracejada(palavra, letrasTentadas.ToList()));
    Console.WriteLine($"Vidas: {vidas}");
    Console.WriteLine($"Letras tentadas:{MontarLetrasTentadas(letrasTentadas)}");

    if (mensagemErro != string.Empty)
    {
        Console.WriteLine();
        Console.WriteLine(mensagemErro);
        Console.WriteLine();
    }

    Console.WriteLine("Digite uma letra:");
    return Console.ReadLine();
}

string MontarPalavraTracejada(string palavra, List<char> letrasTentadas)
{
    var palavraTracejada = string.Empty;
    var letrasTentadasMinusculas = letrasTentadas.Select(letra => char.ToLower(letra)).ToList();

    foreach (char letra in palavra)
    {
        if (letrasTentadasMinusculas.Contains(char.ToLower(letra)))
        {
            palavraTracejada += letra;
            continue;
        }

        palavraTracejada += "_";
    }

    return palavraTracejada;
}

string MontarLetrasTentadas(HashSet<char> letrasTentadas)
{
    var letrasTentadasString = string.Empty;
    var quantidadeLetras = letrasTentadas.Count;
    var letrasTentadasList = letrasTentadas.ToList();

    if (quantidadeLetras <= 0)
        return letrasTentadasString;

    foreach (char letra in letrasTentadas)
    {
        if (letra == letrasTentadasList[quantidadeLetras - 1])
        {
            letrasTentadasString += letra;
            continue;
        }

        letrasTentadasString += $"{letra},";
    }

    return letrasTentadasString;
}

void MontarTelaVitoria(int vidas, string palavraEscolhida)
{
    Console.Clear();
    Console.WriteLine("Parabéns, Você venceu!!!");
    Console.WriteLine($"Vidas:{vidas}");
    Console.WriteLine($"Palavra:{palavraEscolhida}");
    Console.ReadLine();
}

void MontarTelaDerrota(string palavraEscolhida, HashSet<char> letrasTentadas)
{
    Console.Clear();
    Console.WriteLine("Que azar, você perdeu!");
    Console.WriteLine($"Palavra:{palavraEscolhida}");
    Console.WriteLine($"Letras tentadas:{MontarLetrasTentadas(letrasTentadas)}");
    Console.ReadLine();
}

bool VerificarFimDeJogo(int vidas, List<char> letrasTentadas, string palavra)
{
    if (vidas == 0)
        return true;

    var letrasTentadasMinusculas = letrasTentadas.Select(letra => char.ToLower(letra)).ToList();
    var palavraMinuscula = palavra.ToLower();
    var acertouPalavra = palavraMinuscula.All(letra => letrasTentadasMinusculas.Contains(letra));

    return acertouPalavra;
}

bool VerificarChute(string palavra, string chute)
{
    var palavraMinuscula = palavra.ToLower();
    var palavraContemChute = palavraMinuscula.Contains(chute[0]);
    return palavraContemChute;
}

string ValidarChute(string chute, List<char> letrasTentadas)
{
    var regexNumero = new Regex("[0-9]+");

    if (string.IsNullOrWhiteSpace(chute))
        return "A tentativa não pode estar vazia!";

    if (chute.Length > 1)
        return "A tentativa deve ser uma letra!";

    if (regexNumero.IsMatch(chute))
        return "A tentativa não pode ser um número!";

    if (letrasTentadas.Contains(char.ToLower(chute[0])))
        return "Essa letra já foi tentada, escolha outra!";

    return string.Empty;
}


ComecarJogo();