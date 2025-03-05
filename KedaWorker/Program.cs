using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoSauce.MagicScaler;
// using PhotoSauce.NativeCodecs.Giflib;
// using PhotoSauce.NativeCodecs.Libheif;
using PhotoSauce.NativeCodecs.Libjpeg;
// using PhotoSauce.NativeCodecs.Libjxl;
using PhotoSauce.NativeCodecs.Libpng;
// using PhotoSauce.NativeCodecs.Libwebp;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // ✅ Properly configure codecs
        CodecManager.Configure(codecs =>
        {
            // codecs.UseGiflib();
            // codecs.UseLibheif();
            codecs.UseLibjpeg();
            // codecs.UseLibjxl();
            codecs.UseLibpng();
            // codecs.UseLibwebp();
        });

        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();
