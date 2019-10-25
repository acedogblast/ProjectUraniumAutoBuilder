/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ProjectUraniumAutoBuilder;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;
import javafx.scene.control.TextField;
import javafx.stage.DirectoryChooser;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

public class Controller {

    private File pokemonUraniumFolder;
    private File projectUraniumFolder;
    private File ffmpegExe;
    
    
    @FXML
    private Label Lable1;

    @FXML
    private TextField TextF1;

    @FXML
    private Button PokUraFolder;

    @FXML
    private Label Lable2;

    @FXML
    private TextField TextF2;

    @FXML
    private Button ProUraFolder;

    @FXML
    private Button BtnBuild;

    @FXML
    private ComboBox<String> cmbBoxPlatform;

    @FXML
    private Label Lable21;

    @FXML
    private TextField TextF3;

    @FXML
    private Button FFmpegBtn;

    @FXML
    private Label Lable22;
    
    @FXML
    private Label progressLable;
    
    @FXML
    private ProgressBar progressBar;
    
    public void initialize()
    {
        progressLable.setText("");
        cmbBoxPlatform.getItems().addAll(
                "Only copy and fix assets (for devs)"
                //"Linux/X11",
                //"Windows",
                //"MacOS"
        );
    }
    
    @FXML
    void BrowsePokUraFolder(ActionEvent event) {
        DirectoryChooser dc = new DirectoryChooser();
        dc.setTitle("Select Pokemon Uranium Folder");
        pokemonUraniumFolder = dc.showDialog(new Stage());
        TextF1.setText(pokemonUraniumFolder.getAbsolutePath());
    }
    
    @FXML
    void BrowseProUraFolder(ActionEvent event) {
        DirectoryChooser dc = new DirectoryChooser();
        dc.setTitle("Select Project Uranium Folder");
        projectUraniumFolder = dc.showDialog(new Stage());
        TextF2.setText(projectUraniumFolder.getAbsolutePath());
    }
    @FXML
    void BrowseFFmpeg(ActionEvent event) {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Select FFmpeg Executable");
        ffmpegExe = fileChooser.showOpenDialog(new Stage());
        TextF3.setText(ffmpegExe.getAbsolutePath());
    }
    @FXML
    void Build(ActionEvent event) {
        if (pokemonUraniumFolder != null && projectUraniumFolder != null && ffmpegExe != null)
        {
            if (cmbBoxPlatform.getValue().equals("Only copy and fix assets (for devs)"))
            {
                progressLable.setText("Working Please Wait...");
                CopyAndFixAssets();
            }
        }
    }
    private void CopyAndFixAssets() {
        
        // Copy Fonts, Audio, and Graphics
        File projectFonts = new File(projectUraniumFolder.getAbsolutePath() + "/Fonts");
        File pokemonFonts = new File(pokemonUraniumFolder.getAbsoluteFile() + "/Fonts");
        File projectAudio = new File(projectUraniumFolder.getAbsolutePath() + "/Audio");
        File pokemonAudio = new File(pokemonUraniumFolder.getAbsoluteFile() + "/Audio");
        File projectGraphics = new File(projectUraniumFolder.getAbsolutePath() + "/Graphics");
        File pokemonGraphics = new File(pokemonUraniumFolder.getAbsoluteFile() + "/Graphics");
        try
        {
            copyFolder(pokemonFonts, projectFonts);
            copyFolder(pokemonAudio, projectAudio);
            copyFolder(pokemonGraphics, projectGraphics);
        } catch (Exception e)
        {
            e.printStackTrace();
        }
        
        // Delete broken unused files.
        progressLable.setText("Deleting broken unused files");
        File RotatingPieces = new File(projectGraphics.getAbsolutePath() + "/Transitions/RotatingPieces.png");
        RotatingPieces.delete();
        File dialup = new File(projectGraphics.getAbsolutePath() + "/Pictures/dialup.png");
        dialup.delete();
        File copia = new File(projectGraphics.getAbsolutePath() + "/Icons/icon000 - Cópia.png");
        copia.delete();
        copia = new File(projectGraphics.getAbsolutePath() + "/Pictures/Map_icon - Cópia.png");
        copia.delete();
        
        
        
        
        // Fixing Audio files
        String location = projectAudio.getAbsolutePath() + "/SE";
        File computerclose = new File(location + "/computerclose.WAV");
        File computeropen = new File(location + "/computeropen.WAV");
        String arg = "-c:a pcm_s16le";
        
        File[] inputs = new File[1];
        inputs[0] = computerclose;
        RunFFmpeg(inputs, arg, new File(location + "/computerclosePCM.wav"));
        
        inputs[0] = computeropen;
        RunFFmpeg(inputs, arg, new File(location + "/computeropenPCM.wav"));
        computeropen.delete();
        computerclose.delete();
        
        File grasswalk = new File(location + "/PU-Grasswalk.ogg");
        arg = "-c:a copy";
        inputs[0] = grasswalk;
        RunFFmpeg(inputs, arg, new File(location + "/PU-GrasswalkFixed.ogg"));
        grasswalk.delete();
        
        
        // Resizing Tilesets
        
        
        location = projectGraphics.getAbsolutePath() + "/Tilesets/";
        
        ResizeTileset(new File(location + "Cavetiles.png"), 4, 640, "Cavetiles-Resized.png");
        ResizeTileset(new File(location + "foresttiles.png"), 4, 640, "foresttiles-Resized.png");
        ResizeTileset(new File(location + "Indoor(1).png"), 5, 576, "Indoor-Resized.png");
        ResizeTileset(new File(location + "Inside Gyms.png"), 4, 640, "InsideGyms-Resized.png");
        ResizeTileset(new File(location + "NPU-Angelure.png"), 4, 400, "NPU-Angelure-Resized.png");
        ResizeTileset(new File(location + "PU-AngelureFinal.png"), 4, 400, "PU-Angelure-Resized.png");
        ResizeTileset(new File(location + "NPU-Bealbeach.png"), 3, 736, "Bealbeach-Resized.png");
        ResizeTileset(new File(location + "PU-Bealbeach.png"), 3, 736, "PU-Bealbeach-Resized.png");
        ResizeTileset(new File(location + "NPU-Legen Town.png"), 5, 464, "NPU-Legen-Town-Resized.png");
        ResizeTileset(new File(location + "PU-Legen Town.png"), 5, 464, "PU-Legen-Town-Resized.png");
        ResizeTileset(new File(location + "NPU-Nowtoch.png"), 5, 352, "NPU-Nowtoch-Resized.png");
        ResizeTileset(new File(location + "PU-Nowtoch.png"), 5, 352, "PU-Nowtoch-Resized.png");
        ResizeTileset(new File(location + "NPU-Rainforest.png"), 3, 720, "NPU-Rainforest-Resized.png");
        ResizeTileset(new File(location + "PU-Rainforest.png"), 3, 720, "PU-Rainforest-Resized.png");
        ResizeTileset(new File(location + "NPU-Route01-02-Moki-Kevlar.png"), 4, 400, "NPU-Route01-02-Moki-Kevlar-Resized.png");
        ResizeTileset(new File(location + "PU-Route01-02-Moki-Kevlar.png"), 4, 400, "PU-Route01-02-Moki-Kevlar-Resized.png");
        ResizeTileset(new File(location + "PU-Route03-Comet.png"), 4, 560, "PU-Route03-Comet-Resized.png", 338);
        ResizeTileset(new File(location + "NPU-Route03-Comet.png"), 4, 560, "NPU-Route03-Comet-Resized.png", 338);
        ResizeTileset(new File(location + "NPU-Route05-06.png"), 4, 400, "NPU-Route05-06-Resized.png");
        ResizeTileset(new File(location + "PU-Route05-06.png"), 4, 400, "PU-Route05-06-Resized.png");
        ResizeTileset(new File(location + "NPU-Route08.png"), 5, 496, "NPU-Route08-Resized.png");
        ResizeTileset(new File(location + "PU-Route08.png"), 5, 496, "PU-Route08-Resized.png");
        ResizeTileset(new File(location + "NPU-Route09.png"), 4, 400, "NPU-Route09-Resized.png");
        ResizeTileset(new File(location + "PU-Route09.png"), 4, 400, "PU-Route09-Resized.png");
        ResizeTileset(new File(location + "NPU-Route16.png"), 4, 400, "NPU-Route16-Resized.png");
        ResizeTileset(new File(location + "PU-Route16.png"), 4, 400, "PU-Route16-Resized.png");
        ResizeTileset(new File(location + "NPU-Silverport.png"), 4, 400, "NPU-Silverport-Resized.png");
        ResizeTileset(new File(location + "PU-Silverport.png"), 4, 400, "PU-Silverport-Resized.png");
        ResizeTileset(new File(location + "NPU-Tsukinami.png"), 3, 800, "NPU-Tsukinami-Resized.png");
        ResizeTileset(new File(location + "PU-Tsukinami.png"), 3, 800, "PU-Tsukinami-Resized.png");
        ResizeTileset(new File(location + "NPU-Veneza.png"), 4, 464, "NPU-Veneza-Resized.png");
        ResizeTileset(new File(location + "PU-Veneza.png"), 4, 464, "PU-Veneza-Resized.png");
        ResizeTileset(new File(location + "PU-CaveSet.png"), 8, 736, "PU-CaveSet-Resized.png");
        ResizeTileset(new File(location + "PU-Championship.png"), 4, 720, "PU-Championship-Resized.png");
        
        //ResizeTileset(new File(location + "PU-Gym 5.png"), 1, 624, "PU-Gym-5-Resized.png");
        File gym5 = new File(location + "PU-Gym 5.png");
        RunFFmpeg(new File[] {gym5}, "-vf scale=iw/2:ih/2:flags=neighbor", new File(location + "PU-Gym-5-Resized.png"));
        gym5.delete();
        
        
        
        ResizeTileset(new File(location + "PU-Nuclear08.png"), 7, 352, "PU-Nuclear08-Resized.png");
        ResizeTileset(new File(location + "PU-NuclearPlant.png"), 3, 720, "PU-NuclearPlant-Resized.png");
        
        //ResizeTileset(new File(location + "PU-NuclearPlantInside.png"), 1, 1008, "PU-NuclearPlantInside-Resized.png");
        File NuclearInside = new File(location + "PU-NuclearPlantInside.png");
        RunFFmpeg(new File[] {NuclearInside}, "-vf scale=iw/2:ih/2:flags=neighbor", new File(location + "PU-NuclearPlantInside-Resized.png"));
        NuclearInside.delete();
        
        
        ResizeTileset(new File(location + "PU-PowerPlant_2.png"), 3, 800, "PU-PowerPlant_2-Resized.png");
        ResizeTileset(new File(location + "PU-Tsukinami-Indoors.png"), 4, 720, "PU-Tsukinami-Indoors-Resized.png");
        ResizeTileset(new File(location + "PU-Underwater.png"), 2, 544, "PU-Underwater-Resized.png");
        ResizeTileset(new File(location + "PU-VictoryRoad.png"), 3, 720, "PU-VictoryRoad-Resized.png");
        
        // Clean up of unneeded files files.
        File[] unfiles = new File[12];
        unfiles[0] = new File(location + "4.png");
        unfiles[1] = new File(location + "5.png");
        unfiles[2] = new File(location + "6.png");
        unfiles[3] = new File(location + "7.png");
        unfiles[4] = new File(location + "8.png");
        
        unfiles[5] = new File(location + "NPU-Legen_Town.png");
        unfiles[6] = new File(location + "OLDPU-Victory Road.png");
        unfiles[7] = new File(location + "Outside.png");
        unfiles[8] = new File(location + "Outside(new).png");
        unfiles[9] = new File(location + "PU-Angelure.png");
        unfiles[10] = new File(location + "teste.png");
        unfiles[11] = new File(location + "tileset blank.png");
        for (File files : unfiles)
        {
            files.delete();
        }
        
        
        
        progressBar.setProgress(1.0d);
        progressLable.setText("Complete. Have fun!");
    }
    private void ResizeTileset(File input, int columns, int size, String outputName)
    {
        String location = projectUraniumFolder.getAbsolutePath() + "/Graphics/Tilesets/";
        File scaled = new File(location + "scaled.png");
        File[] parts = new File[columns];
        File output = new File(location + outputName);
        for(int i = 1; i <= columns; i ++)
        {
            parts[i - 1] = new File(location + i + ".png");
        }
        
        RunFFmpeg(new File[] {input}, "-vf scale=iw/2:ih/2:flags=neighbor", scaled);
        
        for(int i = 0; i < columns; i++)
        {
            RunFFmpeg(new File[] {scaled}, "-vf crop=128:" + size + ":0:" + size * i, parts[i]);
        }
        RunFFmpeg(parts,"-filter_complex hstack=inputs=" + columns, output);
        
        //System.out.println(output.getAbsolutePath());
        
        input.delete();
        if (outputName.equals("PU-VictoryRoad-Resized.png"))
        {
            scaled.delete();
            for (File file : parts)
            {
                file.delete();
            }
        }
    }
    private void ResizeTileset(File input, int columns, int size, String outputName, int paddedSize)
    {
        String location = projectUraniumFolder.getAbsolutePath() + "/Graphics/Tilesets/";
        File scaled = new File(location + "scaled.png");
        File[] parts = new File[columns];
        File output = new File(location + outputName);
        for(int i = 1; i <= columns; i ++)
        {
            parts[i - 1] = new File(location + i + ".png");
        }
        RunFFmpeg(new File[] {input}, "-vf scale=iw/2:ih/2:flags=neighbor", scaled);
        
        for(int i = 0; i < columns - 1; i++)
        {
            RunFFmpeg(new File[] {scaled}, "-vf crop=128:" + size + ":0:" + size * i, parts[i]);
        }
        RunFFmpeg(new File[] {scaled}, "-vf crop=128:" + paddedSize + ":0:" + size * (columns - 1) + ",pad=128:" + size + ":0:0:0x000000@0x00", parts[columns - 1]);
        
        
        
        RunFFmpeg(parts,"-filter_complex hstack=inputs=" + columns, output);
        input.delete();
    }
    private void RunFFmpeg(File[] inputs, String args, File output)
    {
        String[] splitArgs = args.split(" ");
        
        String[] inputFormatted = new String[inputs.length * 2];
        int index = 0;
        for(int i = 0; i < inputs.length; i++)
        {
            inputFormatted[index] = "-i";
            inputFormatted[index + 1] = inputs[i].getAbsolutePath();
            index += 2;
        }
        String[] comand = new String[splitArgs.length + 3 + inputFormatted.length];
        comand[0] = ffmpegExe.getAbsolutePath();
        comand[1] = "-y";
        for (int i = 0; i < inputFormatted.length; i++)
        {
            comand[2 + i] = inputFormatted[i];
        }
        for (int i = 0; i < splitArgs.length; i++)
        {
            comand[i + 2 + inputs.length * 2] = splitArgs[i];
        }
        comand[comand.length - 1] = output.getAbsolutePath();
        
        try 
        {
            Process ffmpeg = Runtime.getRuntime().exec(comand);
            ffmpeg.waitFor();
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }
        
    private void copyFolder(File sourceFolder, File destinationFolder) throws IOException
    {
        //Check if sourceFolder is a directory or file
        //If sourceFolder is file; then copy the file directly to new location
        if (sourceFolder.isDirectory())
        {
            //Verify if destinationFolder is already present; If not then create it
            if (!destinationFolder.exists())
            {
                destinationFolder.mkdir();
                //System.out.println("Directory created :: " + destinationFolder);
            }
             
            //Get all files from source directory
            String files[] = sourceFolder.list();
             
            //Iterate over all files and copy them to destinationFolder one by one
            for (String file : files)
            {
                File srcFile = new File(sourceFolder, file);
                File destFile = new File(destinationFolder, file);
                 
                //Recursive function call
                copyFolder(srcFile, destFile);
            }
        }
        else
        {
            //Copy the file content from one place to another
            Files.copy(sourceFolder.toPath(), destinationFolder.toPath(), StandardCopyOption.REPLACE_EXISTING);
            //System.out.println("File copied :: " + destinationFolder);
        }
    }
  
}