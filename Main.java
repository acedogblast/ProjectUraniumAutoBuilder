
package ProjectUraniumAutoBuilder;

import javafx.application.Application;
import javafx.scene.*;
import javafx.stage.*;
import javafx.scene.layout.VBox;
import javafx.fxml.FXMLLoader;
/**
 *
 * @author acedogblast
 */
public class Main extends Application {

    public static void main(String[] args) {
        launch(args);
    }
    public void start(Stage primaryStage) throws Exception
    {
        primaryStage.setTitle("Project Uranium Auto-Builder");
        
        VBox fxml = (VBox) FXMLLoader.load(getClass().getResource("/ProjectUraniumAutoBuilderScene.fxml"));
        Scene scene = new Scene(fxml);
        primaryStage.setScene(scene);
        primaryStage.setResizable(false);
        primaryStage.show();
    }
}
