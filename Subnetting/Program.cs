/*
 * IPV4 Subnet calculator. WIP
 *
 * Lang: IT
 * 
 * Descrizione: Richiede il seguente input da parte dell'utente:
 * - startingNet: L'indirizzo IP della rete della quale effettuare il subnetting, espresso in formato x.x.x.x/sm
 * - subnetCount: La quantità di subnet da creare
 * - hosts[]: Di ciascuna subnet, il numero massimo di dispositivi previsti
 * 
 * Allo stato attuale non viene gestito nessun input errato da parte dell'utente.
 * 
 * Author: Edoardo Mileto
 */

int question = 6 / 8;

Console.WriteLine(question);

char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

/*
 * Questo array contiene la conversione in decimale del numero di bit messi a 1 corrispondenti al numero di index dell'array
 * Esempio: 3 bit tutti a 1 = 7 -> broadcastList[3] = 7
 */
int[] broadcastList = {0, 1, 3, 7, 15, 31, 63, 127, 255};

Console.Write("\nQuale rete stai usando? Usa il formato x.x.x.x/sm: ");

string startingNet = Console.ReadLine().Trim(); //todo tryparse

//spezzo la stringa ricevuta in input separando IP e bit usati per la Subnet Mask 
string[] networkSplitted = startingNet.Split("/");

string ip = networkSplitted[0];
int sm = Convert.ToInt32(networkSplitted[1]);

Console.WriteLine($"Con questa rete puoi avere al massimo {Math.Pow(2, 32 - sm)} host");

string[] ipSplitted = ip.Split(".");

string ipNetwork;
string[] ipHost;

switch (sm)
{
    case (8):
        ipNetwork = ipSplitted[0];
        ipHost = new[] {ipSplitted[1], ipSplitted[2], ipSplitted[3]};
        break;
    case (16):
        ipNetwork = string.Concat(ipSplitted[0], "." , ipSplitted[1]);
        ipHost = new[] {ipSplitted[2], ipSplitted[3]};
        break;
    case (24):
        ipNetwork = string.Concat(ipSplitted[0], ".", ipSplitted[1], ".", ipSplitted[2]);
        ipHost = new[] {ipSplitted[3]};
        break;
    default:
        Console.WriteLine("errore nella suddivisione della rete");
        break;
}

Console.Write("\nQuante sottoreti devi creare? ");

int subnetCount = int.Parse(Console.ReadLine()); //todo tryparse
int[] hosts = new int[subnetCount];
string[] subnets = new string[subnetCount];
string[] broadcasts = new string[subnetCount];

subnets[0] = ip;

Console.WriteLine("\nInserisci il numero massimo di dispositivi previsti per ogni sottorete\n");

for (var i = 0; i < subnetCount; i++)
{
    Console.Write($"Sottorete {alphabet[i]}: ");
    hosts[i] = int.Parse(Console.ReadLine()); //todo tryparse
}

//Non c'è un metodo che ordini direttamente il vettore in ordine discendente?
Array.Sort(hosts);

Array.Reverse(hosts);

int[] usedBits = CalcUsedBits(hosts);


for (int i = 0; i < subnetCount; i++)
{
    int tmp;
    switch (usedBits[i] / 8)
    {
        case 3:
            if (usedBits[i] % 24 == 0)
            {
                goto case 2;
            }
            else
            {
                Console.WriteLine("ciao sono nel case 3");
                tmp = usedBits[i] - 24;
                tmp = broadcastList[tmp];
                Console.WriteLine(tmp);
                break;
            }
        case 2:
            if (usedBits[i] % 16 == 0)
            {
                goto case 1;
            }
            else
            {
                Console.WriteLine("ciao sono nel case 2");
                tmp = usedBits[i] - 16;
                tmp = broadcastList[tmp];
                Console.WriteLine(tmp);
                break;
            }

        case 1:
            if (usedBits[i] % 8 == 0)
            {
                goto case 0;
            }
            else
            {
                Console.WriteLine("ciao sono nel case 1");
                tmp = usedBits[i] - 8;
                tmp = broadcastList[tmp];
                Console.WriteLine(tmp);
                break;
            }
        case 0:
            Console.WriteLine("ciao sono nel case 0");
            tmp = broadcastList[usedBits[i]];
            Console.WriteLine(tmp);
            break;
    }
}


Console.WriteLine("\nNome Rete\tn° hosts\tbit usati\tsubnet mask\tsubnet\t\tbroadcast");
for (int i = 0; i < subnetCount; i++)
{
    Console.WriteLine(
        $"{alphabet[i]}\t\t{hosts[i]}\t\t{usedBits[i]}\t\t/{32 - usedBits[i]}\t\t{subnets[i]}/{32 - usedBits[i]}");
}

//todo sort hostsArray

static int[] CalcUsedBits(int[] hosts)
{
    var usedBits = new int[hosts.Length];

    for (int i = 0; i < hosts.Length; i++)
    {
        usedBits[i] = (int) Math.Ceiling(Math.Log2(hosts[i] + 2));
    }

    return usedBits;
}

/*static string[] CalcBroadcast()
{
    
}*/