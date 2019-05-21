// Cargar el modulo HTTP
var http = require('http');

//firebase admin
var admin = require('firebase-admin');

var serviceAccount = require('./depi-belle-firebase-adminsdk.json.js');

admin.initializeApp({
credential: admin.credential.cert(serviceAccount),
databaseURL: 'https://depi-belle.firebaseio.com'
});

// Configurar una respuesta HTTP para todas las peticiones
function onRequest(request, response) {
    console.log("Peticion Recibida.");
    response.writeHead(200, {"Content-Type": "text/html"});
    response.write("Hola Mundo");
    response.end();

    var userEmail = "r.pintos.banchero@gmail.com"//"contacto@depi-belle.com"


    //GIVE PERMISSIONS

    /*
    admin.auth().getUserByEmail(userEmail).then((user) => {
        // Confirm user is verified.
        //if (user.emailVerified) {
            // Add custom claims for additional privileges.
            // This will be picked up by the user on token refresh or next sign in on new device.
            return admin.auth().setCustomUserClaims(user.uid, {
                admin: true
            });
        //}
    }).catch((error) => {
        console.log(error);
    });
    */

    //QUERY USER 

    /*
    admin.auth().getUserByEmail(userEmail)
    .then(function(userRecord) {
        // See the UserRecord reference doc for the contents of userRecord.
        console.log("Successfully fetched user data:", userRecord.toJSON());
    })
    .catch(function(error) {
        console.log("Error fetching user data:", error);
    });
    */

    //VERIFY TOKEN
    //el token debe haber sido pedido una vez que el usuario ya tenia los permisos
    
    /*
    var idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImZmMWRmNWExNWI1Y2Y1ODJiNjFhMjEzODVjMGNmYWVkZmRiNmE3NDgiLCJ0eXAiOiJKV1QifQ.eyJtb2RlcmF0b3IiOnRydWUsImlzcyI6Imh0dHBzOi8vc2VjdXJldG9rZW4uZ29vZ2xlLmNvbS9kZXBpLWJlbGxlIiwiYXVkIjoiZGVwaS1iZWxsZSIsImF1dGhfdGltZSI6MTU1NDU4Mjk0MSwidXNlcl9pZCI6IlJ4WHpoc0ppNFBSWm5yNmtINHFTSG1pdWg2MzMiLCJzdWIiOiJSeFh6aHNKaTRQUlpucjZrSDRxU0htaXVoNjMzIiwiaWF0IjoxNTU0NTgyOTQxLCJleHAiOjE1NTQ1ODY1NDEsImVtYWlsIjoici5waW50b3MuYmFuY2hlcm9AZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbInIucGludG9zLmJhbmNoZXJvQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.FBnzleytVUofOIZ-TkLClUC7f6nWJqifCOPtnk4yNHgSpPi8q3NzvEBlqoO_u7iDmdsFk5AW1Q15FEjBb0ZXVINZMYxUxEuayaYi9Ldlf2NAoabLRuBLUB1fy3AJNOWTf7DKJEGnxqNNPSFubn4EJX7Y2MFClYvAnOypdgnnCNHoud237-e0LN9vcgGuYdJqj52CA8V1c-fC3ohew7fsUmghbx9DpiyWDGwsCh61gEm1Z2I-gfAT8QxmpCpaTW8Q-9s9_aQ0nIdrEOQIfLWa5e04YEcnoYJ3z8dPxF7naLkoVQj60Up_P4fZL0WdGMuA787ibE-U8P8jki-CJrjfIQ"
    //var idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdkMmY5ZjNmYjgzZDYzMzc0OTdiNmY3Y2QyY2ZmNGRmYTVjMmU4YjgiLCJ0eXAiOiJKV1QifQ.eyJhZG1pbiI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2RlcGktYmVsbGUiLCJhdWQiOiJkZXBpLWJlbGxlIiwiYXV0aF90aW1lIjoxNTU0NTgxOTU3LCJ1c2VyX2lkIjoiZ3NKSmUzd1ZVUmhVYWRObXhXWGljZ2lKOXFHMiIsInN1YiI6ImdzSkplM3dWVVJoVWFkTm14V1hpY2dpSjlxRzIiLCJpYXQiOjE1NTQ1ODE5NTcsImV4cCI6MTU1NDU4NTU1NywiZW1haWwiOiJjb250YWN0b0BkZXBpLWJlbGxlLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImNvbnRhY3RvQGRlcGktYmVsbGUuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.y5OTojAwVwUZFYjmCfGVk1a29caF_EjuylqwGYc-m6IXJsfxUzlsWSp4HBPU_3kOHrBZKekEYGGPDYR30Rn4AyDynRWALFHLChfPc-Yz1Twk_EFpzDoachmDbF9xbc8mUFWf1v9rOoeHYrAzR29x-1HhI6fEBrvTRiqjqKXgADOSMDZtn4wv3TF5Fdk07zrCX7YamkWaA88cYgQhiSqRiStvYfkzyQNnqKyRsZZ6B24HlQySMyyfiGTeIAHa9b5Gu1GYzWVdxbvrZTivvortLqwK4Vb3MRnXAuV3lpe8oGXiFaQElSmjaTa_ZweLuyOFjnukp6RkuK7Xx8VMLm8qEA"

    admin.auth().verifyIdToken(idToken)
    .then(function(decodedToken) {
        console.log(JSON.stringify(decodedToken));
        var uid = decodedToken.uid;

        if(decodedToken.moderator)
            console.log("es moderator")

    }).catch(function(error) {
        // Handle error
        console.log(error);
    });
    */
   
    //CONSULTAR UN NODO DE LA BASE

    /*
    var db = admin.database();
    var ref = db.ref("dev/promotions");
    ref.once("value", function(snapshot) {
        console.log(snapshot.val());
    });
    */
}
 
var server = http.createServer(onRequest);
 
// Escuchar al puerto 8080
server.listen(8080);
 
// Poner un mensaje en la consola
console.log("Servidor funcionando en http://localhost:8080/");

