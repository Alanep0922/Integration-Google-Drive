using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

var credencial = GoogleCredential.FromFile(@"./Credencial.json")
    .CreateScoped(DriveService.ScopeConstants.Drive);

var service = new DriveService(new BaseClientService.Initializer()
{
    HttpClientInitializer = credencial,
});

CriarArquivo("meuarquivo", "hello world");


void CriarArquivo(string nomeDoArquivo, string conteudoDoArquivo)
{
    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
    {
        Name = nomeDoArquivo,
    };
    var mediaContent = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(conteudoDoArquivo));
    var uploadRequest = service.Files.Create(fileMetadata, mediaContent, "text/plain");
    uploadRequest.Fields = "*";
    var progress = uploadRequest.Upload();

    progress.ThrowOnFailure();

    Console.WriteLine($"Upload do Arquivo foi executado com sucesso!. O Id do arquivo é: {uploadRequest.ResponseBody.Id}");

}
void ExcluirArquivo(string arquivoId)
{
    var deleteRequest = service.Files.Delete(arquivoId);
    var progress = deleteRequest.Execute();
    Console.WriteLine($"O arquivo com o Id {arquivoId} foi excluído com sucesso!");
}
ExcluirArquivo("1jh0Fietju12Kz4Hl2tL39-FW5B_MBvOI");

void BaixarArquivo(string arquivoId, string caminhoLocal)
{
    var getRequest = service.Files.Get(arquivoId);
    using (var arquivoStream = new System.IO.FileStream(caminhoLocal, System.IO.FileMode.Create))
    {
        getRequest.Download(arquivoStream);
    }
    Console.WriteLine($"O arquivo com o Id {arquivoId} foi baixado com sucesso para o caminho {caminhoLocal}!");
}
BaixarArquivo("1AB5p95sE1NQGdAlb_Q1aPjSs0I3BEtsz", "./credecial");

 void ListarArquivos()
{
    var request = service.Files.List();
    request.PageSize = 10;
    request.Fields = "nextPageToken, files(id, name)";

    var result = request.Execute();
    Console.WriteLine("Arquivos no Google Drive:");
    if (result.Files != null && result.Files.Any())
    {
        foreach (var file in result.Files)
        {
            Console.WriteLine("{0} ({1})", file.Name, file.Id);
        }
    }
    else
    {
        Console.WriteLine("Nenhum arquivo encontrado no Google Drive.");
    }
}
ListarArquivos();
