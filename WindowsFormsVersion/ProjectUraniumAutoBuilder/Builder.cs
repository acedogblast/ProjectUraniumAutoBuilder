using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ProjectUraniumAutoBuilder
{
    public class Builder
    {
        private String OrginalPokemonUraniumFolder, ProjectUraniumGodotFolder, FFmpegExe, ExportFolder;
        public void setupAssets(IProgress<int> progress, String Original, String Project, String FFmpeg)
        {
            OrginalPokemonUraniumFolder = Original;
            ProjectUraniumGodotFolder = Project;
            FFmpegExe = FFmpeg;

            int percentComplete = 0;
            progress.Report(percentComplete);

            DirectoryInfo projectFonts = new DirectoryInfo(ProjectUraniumGodotFolder + @"\Fonts");
            DirectoryInfo pokemonFonts = new DirectoryInfo(OrginalPokemonUraniumFolder + @"\Fonts");
            DirectoryInfo projectAudio = new DirectoryInfo(ProjectUraniumGodotFolder + @"\Audio");
            DirectoryInfo pokemonAudio = new DirectoryInfo(OrginalPokemonUraniumFolder + @"\Audio");
            DirectoryInfo projectGraphics = new DirectoryInfo(ProjectUraniumGodotFolder + @"\Graphics");
            DirectoryInfo pokemonGraphics = new DirectoryInfo(OrginalPokemonUraniumFolder + @"\Graphics");

            // Copy Fonts, Audio, and Graphics Folders
            CopyAll(pokemonFonts, projectFonts);

            percentComplete = 10;
            progress.Report(percentComplete);

            CopyAll(pokemonAudio, projectAudio);

            percentComplete = 30;
            progress.Report(percentComplete);

            CopyAll(pokemonGraphics, projectGraphics);

            percentComplete = 40;
            progress.Report(percentComplete);

            // Delete broken unused files.
            if (File.Exists(projectGraphics.FullName + @"\Transitions\RotatingPieces.png"))
            {
                File.Delete(projectGraphics.FullName + @"\Transitions\RotatingPieces.png");
            }
            if (File.Exists(projectGraphics.FullName + @"\Pictures\dialup.png"))
            {
                File.Delete(projectGraphics.FullName + @"\Pictures\dialup.png");
            }
            if (File.Exists(projectGraphics.FullName + @"\Icons\icon000 - Cópia.png"))
            {
                File.Delete(projectGraphics.FullName + @"\Icons\icon000 - Cópia.png");
            }
            if (File.Exists(projectGraphics.FullName + @"\Pictures\Map_icon - Cópia.png"))
            {
                File.Delete(projectGraphics.FullName + @"\Pictures\Map_icon - Cópia.png");
            }

            percentComplete = 45;
            progress.Report(percentComplete);

            // Fix Audio files
            String location = projectAudio + @"\SE";
            String computerclose = location + @"\computerclose.WAV";
            String computeropen = location + @"\computeropen.WAV";
            String arg = "-c:a pcm_s16le";
            String[] inputs = new string[1];
            inputs[0] = computerclose;
            RunFFmpeg(inputs, arg, location + @"\computerclosePCM.wav");
            inputs[0] = computeropen;
            RunFFmpeg(inputs, arg, location + @"\computeropenPCM.wav");

            File.Delete(computerclose);
            File.Delete(computeropen);

            String grasswalk = location + @"\PU-Grasswalk.ogg";
            arg = "-c:a copy";
            inputs[0] = grasswalk;
            RunFFmpeg(inputs, arg, location + @"\PU-GrasswalkFixed.wav");
            File.Delete(grasswalk);

            percentComplete = 50;
            progress.Report(percentComplete);

            // Resizing Tilesets

            location = projectGraphics + @"\Tilesets\";

            ResizeTileset(location + "Cavetiles.png", 4, 640, "Cavetiles-Resized.png");
            ResizeTileset(location + "foresttiles.png", 4, 640, "foresttiles-Resized.png");
            percentComplete = 52;
            progress.Report(percentComplete);
            ResizeTileset((location + "Indoor(1).png"), 5, 576, "Indoor-Resized.png");
            ResizeTileset((location + "Inside Gyms.png"), 4, 640, "InsideGyms-Resized.png");
            percentComplete = 54;
            progress.Report(percentComplete);
            ResizeTileset((location + "NPU-Angelure.png"), 4, 400, "NPU-Angelure-Resized.png");
            ResizeTileset((location + "PU-AngelureFinal.png"), 4, 400, "PU-Angelure-Resized.png");
            percentComplete = 56;
            progress.Report(percentComplete);
            ResizeTileset((location + "NPU-Bealbeach.png"), 3, 736, "Bealbeach-Resized.png");
            ResizeTileset((location + "PU-Bealbeach.png"), 3, 736, "PU-Bealbeach-Resized.png");
            percentComplete = 58;
            progress.Report(percentComplete);
            ResizeTileset((location + "NPU-Legen Town.png"), 5, 464, "NPU-Legen-Town-Resized.png");
            ResizeTileset((location + "PU-Legen Town.png"), 5, 464, "PU-Legen-Town-Resized.png");
            percentComplete = 60;
            progress.Report(percentComplete);
            ResizeTileset((location + "NPU-Nowtoch.png"), 5, 352, "NPU-Nowtoch-Resized.png");
            ResizeTileset((location + "PU-Nowtoch.png"), 5, 352, "PU-Nowtoch-Resized.png");
            percentComplete = 62;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Rainforest.png"), 3, 720, "PU-Rainforest-Resized.png");
            ResizeTileset((location + "NPU-Route01-02-Moki-Kevlar.png"), 4, 400, "NPU-Route01-02-Moki-Kevlar-Resized.png");
            percentComplete = 64;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Route01-02-Moki-Kevlar.png"), 4, 400, "PU-Route01-02-Moki-Kevlar-Resized.png");
            ResizeTileset((location + "PU-Route03-Comet.png"), 4, 560, "PU-Route03-Comet-Resized.png", 338);
            percentComplete = 66;
            progress.Report(percentComplete);
            ResizeTileset((location + "NPU-Route03-Comet.png"), 4, 560, "NPU-Route03-Comet-Resized.png", 338);
            ResizeTileset((location + "NPU-Route05-06.png"), 4, 400, "NPU-Route05-06-Resized.png");
            percentComplete = 68;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Route05-06.png"), 4, 400, "PU-Route05-06-Resized.png");
            ResizeTileset((location + "NPU-Route08.png"), 5, 496, "NPU-Route08-Resized.png");
            percentComplete = 70;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Route08.png"), 5, 496, "PU-Route08-Resized.png");
            ResizeTileset((location + "NPU-Route09.png"), 4, 400, "NPU-Route09-Resized.png");
            percentComplete = 72;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Route09.png"), 4, 400, "PU-Route09-Resized.png");
            ResizeTileset((location + "NPU-Route16.png"), 4, 400, "NPU-Route16-Resized.png");
            percentComplete = 74;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Route16.png"), 4, 400, "PU-Route16-Resized.png");
            ResizeTileset((location + "NPU-Silverport.png"), 4, 400, "NPU-Silverport-Resized.png");
            percentComplete = 76;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Silverport.png"), 4, 400, "PU-Silverport-Resized.png");
            ResizeTileset((location + "NPU-Tsukinami.png"), 3, 800, "NPU-Tsukinami-Resized.png");
            percentComplete = 78;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Tsukinami.png"), 3, 800, "PU-Tsukinami-Resized.png");
            ResizeTileset((location + "NPU-Veneza.png"), 4, 464, "NPU-Veneza-Resized.png");
            percentComplete = 80;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Veneza.png"), 4, 464, "PU-Veneza-Resized.png");
            ResizeTileset((location + "PU-CaveSet.png"), 8, 736, "PU-CaveSet-Resized.png");
            percentComplete = 82;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Championship.png"), 4, 720, "PU-Championship-Resized.png");

            String gym5 = location + "PU-Gym 5.png"; // Gym 5 is small enough to not need to be vstack.
            RunFFmpeg(new string[] { gym5 }, "-vf scale=iw/2:ih/2:flags=neighbor", (location + "PU-Gym-5-Resized.png"));
            File.Delete(gym5);

            percentComplete = 84;
            progress.Report(percentComplete);

            ResizeTileset((location + "PU-Nuclear08.png"), 7, 352, "PU-Nuclear08-Resized.png");
            ResizeTileset((location + "PU-NuclearPlant.png"), 3, 720, "PU-NuclearPlant-Resized.png");

            percentComplete = 86;
            progress.Report(percentComplete);

            String NuclearInside = location + "PU-NuclearPlantInside.png"; // NuclearInside is also small enough to not need to be vstack.
            RunFFmpeg(new string[] { NuclearInside }, "-vf scale=iw/2:ih/2:flags=neighbor", location + "PU-NuclearPlantInside-Resized.png");
            File.Delete(NuclearInside);


            ResizeTileset((location + "PU-PowerPlant_2.png"), 3, 800, "PU-PowerPlant_2-Resized.png");
            percentComplete = 88;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-Tsukinami-Indoors.png"), 4, 720, "PU-Tsukinami-Indoors-Resized.png");
            ResizeTileset((location + "PU-Underwater.png"), 2, 544, "PU-Underwater-Resized.png");
            percentComplete = 90;
            progress.Report(percentComplete);
            ResizeTileset((location + "PU-VictoryRoad.png"), 3, 720, "PU-VictoryRoad-Resized.png");

            // Clean up of unneeded files
            File.Delete(location + "4.png");
            File.Delete(location + "5.png");
            File.Delete(location + "6.png");
            File.Delete(location + "7.png");
            File.Delete(location + "8.png");
            File.Delete(location + "NPU-Legen_Town.png");
            File.Delete(location + "OLDPU-Victory Road.png");
            File.Delete(location + "Outside.png");
            File.Delete(location + "Outside(new).png");
            File.Delete(location + "PU-Angelure.png");
            File.Delete(location + "teste.png");
            File.Delete(location + "tileset blank.png");
            percentComplete = 100;
            progress.Report(percentComplete);

            File.Create(ProjectUraniumGodotFolder + @"\SetUpComplete.txt");
        }
        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        private void RunFFmpeg(String[] inputs, String args, String output)
        {
            String[] splitArgs = args.Split(' ');
            String[] inputFormatted = new string[inputs.Length * 2];

            int index = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                inputFormatted[index] = "-i";
                inputFormatted[index + 1] = inputs[i];
                index += 2;
            }

            String[] comand = new String[splitArgs.Length + 2 + inputFormatted.Length];
            comand[0] = "-y";

            for (int i = 0; i < inputFormatted.Length; i++)
            {
                comand[1 + i] = inputFormatted[i];
            }
            for (int i = 0; i < splitArgs.Length; i++)
            {
                comand[i + 1 + inputs.Length * 2] = splitArgs[i];
            }
            comand[comand.Length - 1] = output;

            Process ffmpeg = new Process();
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.FileName = FFmpegExe;
            ffmpeg.StartInfo.Arguments = String.Join(" ", comand);
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }
        private void ResizeTileset(String input, int columns, int size, String outputName)
        {
            String location = ProjectUraniumGodotFolder + @"\Graphics\Tilesets\";
            String scaled = location + "scaled.png";
            String[] parts = new String[columns];
            String output = location + outputName;

            for (int i = 1; i <= columns; i++)
            {
                parts[i - 1] = location + i + ".png";
            }

            RunFFmpeg(new string[] { input }, "-vf scale=iw/2:ih/2:flags=neighbor", scaled);

            for (int i = 0; i < columns; i++)
            {
                RunFFmpeg(new string[] { scaled }, "-vf crop=128:" + size + ":0:" + size * i, parts[i]);
            }
            RunFFmpeg(parts, "-filter_complex hstack=inputs=" + columns, output);

            File.Delete(input);

            if (outputName.Equals("PU-VictoryRoad-Resized.png"))
            {
                File.Delete(scaled);

                foreach (String file in parts)
                {
                    File.Delete(file);
                }

            }
        }
        private void ResizeTileset(String input, int columns, int size, String outputName, int paddedSize)
        {
            String location = ProjectUraniumGodotFolder + @"\Graphics\Tilesets\";
            String scaled = location + "scaled.png";
            String[] parts = new String[columns];
            String output = location + outputName;

            for (int i = 1; i <= columns; i++)
            {
                parts[i - 1] = location + i + ".png";
            }

            RunFFmpeg(new string[] { input }, "-vf scale=iw/2:ih/2:flags=neighbor", scaled);

            for (int i = 0; i < columns; i++)
            {
                RunFFmpeg(new string[] { scaled }, "-vf crop=128:" + size + ":0:" + size * i, parts[i]);
            }

            RunFFmpeg(new String[] { scaled }, "-vf crop=128:" + paddedSize + ":0:" + size * (columns - 1) + ",pad=128:" + size + ":0:0:0x000000@0x00", parts[columns - 1]);

            RunFFmpeg(parts, "-filter_complex hstack=inputs=" + columns, output);

            File.Delete(input);
        }
    }
}
