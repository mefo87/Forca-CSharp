//mostrar um menu pra começar o jogo

using System.Text.RegularExpressions;

string? Menu()
{
    Console.Clear();
    Console.WriteLine("---- Bem-vindo ao Jogo da Forca ----");
    Console.WriteLine();
    Console.WriteLine("1 - Começar jogo");
    Console.WriteLine("0 - Sair");

    return Console.ReadLine();
}


void Jogo()
{
    var escolha = Menu();

    while (escolha != "0")
    {
        if (escolha == "1")
        {
            IniciarJogo();
        }

        escolha = Menu();
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
    var chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, string.Empty);
    var regexNumero = new Regex("[0-9]+");
    var chuteValido = true;

    while (chuteValido == false)
    {
        if (string.IsNullOrWhiteSpace(chute))
        {
            chuteValido = false;
            chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, "A tentativa não pode estar vazia!");
            continue;
        }

        if (chute.Length > 1)
        {
            chuteValido = false;
            chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, "A tentativa deve ser uma letra!");
            continue;
        }

        if (regexNumero.IsMatch(chute))
        {
            chuteValido = false;
            chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, "A tentativa não pode ser um número!");
            continue;
        }

        if (letrasTentadas.Contains(chute[0]))
        {
            chuteValido = false;
            chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, "A tentativa já foi feita, escolha outra letra!");
            continue;
        }

        chuteValido = true;
    }

    var chuteCorreto = VerificarChute(palavraEscolhida, chute[0]);

    if (chuteCorreto)
    {
        letrasTentadas.Add(chute[0]);
        chute = MontarTelaJogo(palavraEscolhida, vidas, letrasTentadas, string.Empty);
    }


}

//mostrar a tela inicial com a palavra tracejada, a quantidade de vidas e quais letras foram testadas
//receber uma letra e vai verificar se essa letra existe na palavra escolhida
//caso não tenha a letra, perde uma vida, senao mostra a letra escolhida no lugar correto dentro da palavra

string? MontarTelaJogo(string palavra, int vidas, HashSet<char> letrasTentadas, string mensagemErro)
{
    Console.Clear();
    Console.WriteLine(" - - - Jogo da Forca - - - ");
    Console.WriteLine();
    Console.WriteLine(MontarPalavraTracejada(palavra, letrasTentadas));
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

string MontarPalavraTracejada(string palavra, HashSet<char> letrasTentadas)
{
    var palavraTracejada = string.Empty;

    foreach(char letra in palavra)
    {
        if (letrasTentadas.Contains(letra))
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

    if (quantidadeLetras > 0)
        return letrasTentadasString;

    foreach (char letra in letrasTentadas)
    {
        if (letra == letrasTentadasList[quantidadeLetras - 1])
        {
            letrasTentadasString += letra;
            continue;
        }

        letrasTentadasString += $"{letra}, ";
    }

    return letrasTentadasString;
}

bool VerificarChute(string palavra, char tentativa)
{
    return palavra.Contains(tentativa);
}

//caso o numero de vidas seja 0, termina o jogo e mostra a palavra completa
//caso a palavra esteja correta, termina o jogo e mostra um parabens
//retorna ao menu principal.



Jogo();