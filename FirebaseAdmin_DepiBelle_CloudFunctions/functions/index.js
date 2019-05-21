const functions = require('firebase-functions');
const admin = require('firebase-admin');
var serviceAccount = require('./depi-belle-firebase-adminsdk.json');
admin.initializeApp({
   credential: admin.credential.cert(serviceAccount),
   databaseURL: 'https://depi-belle.firebaseio.com'
});

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions

const dev_path =  "environment/dev/"

 exports.helloFromDepiBelle= functions.https.onRequest((request, response) => {
    console.log("holaaa");
    response.send("Hello from DepiBelle!");
 });

 exports.devDepilatorRegisterToken = functions.firestore.document(dev_path+"depiladoras/{depi}").onCreate((snap, context) => {
   
   console.log("va a registrar token en topico depiladora")
   admin.messaging().subscribeToTopic([snap.id], "depiladora")
  .then(response=> {
    console.log('Successfully subscribed to topic:', response);
    return "ok"
  })
  .catch(function(error) {
    console.log('Error subscribing to topic:', error);
    throw error;
  });

   return "ok";
   
});

 exports.devPendingOrderAdded = functions.firestore.document(dev_path+"orders/ordersInProcess/{date}/{order}").onCreate((snap, context) => {
   
   //SEND TO DEVICES NO ADMITE EN MENSAJE ENVIAR ALGO ESPECIFICO DE ANDROID Y IOS 
   //PERO ADMITE MANDAR A ARRAY DE TOKENS
   /* 
   console.log("se creoooo orden");
    var notification = {title:"DepiBelle - Nueva Orden", body:"Hay trabajo, a laburar"}//{{data:{title:"DepiBelle - Nueva Orden", body:"Hay trabajo, a laburar"}}
    var message = {notification:notification,data:{depi_belle:"true"}};

   // Get the list of device tokens.
   admin.firestore().collection(dev_path+"depiladoras").get().then(result=>
   {  
      const tokens = [];

      result.forEach(function(doc) {
         // doc.data() is never undefined for query doc snapshots
         //console.log(doc.id, " => ", doc.data());
         tokens.push(doc.data().pushToken);
      });

      if (tokens.length > 0) {
         // Send notifications to all tokens.
         console.log(tokens[0]);
         const response = admin.messaging().sendToDevice(tokens, message);
         console.log("se envio");
      }
      else{
         console.log("no hay tokens");
      }

      return "ok";
   })
   .catch(function(error) {
      console.log("Error getting documents: ", error);
      throw error;
   });

   
   return "ok";*/

   //SEND TO DEVICES ADMITE EN MENSAJE ENVIAR ALGO ESPECIFICO DE ANDROID Y IOS
   //PERO NO A ARRAY DE DEVICES
   /*
   console.log("se creoooo orden");
   var notification = {data:{title:"DepiBelle - Nueva Orden", body:"Hay trabajo, a laburar"}}//{title:"DepiBelle - Nueva Orden", body:"Hay trabajo, a laburar"}//{
   var message = {android:notification,data:{depibelle_depi:"true"}};

  // Get the list of device tokens.
  admin.firestore().collection(dev_path+"depiladoras").get()
  .then(result=>
  {  
     result.forEach(function(doc) {
        message.token=doc.data().pushToken;
        const response = admin.messaging().send(message);
     });

     return "ok";
  })
  .catch(function(error) {
     console.log("Error getting documents: ", error);
     throw error;
  });

  
  return "ok";
   */
  
  //ENVIO DE PUSH A TOPICO
  console.log("se creoooo orden");
  var notification = {data:{title:"Nueva Orden - "+snap.data().name, body:"Hay trabajo, a laburar"}}//{title:"DepiBelle - Nueva Orden", body:"Hay trabajo, a laburar"}//{
  var message = {android:notification,
                  data:{
                     depibelle_depi:"true",
                     linkeableItem:JSON.stringify(
                        {  type:"1",
                           link:JSON.stringify({navTo:"NUEVA_ORDEN",param:'{orderId:"'+snap.id+'"}'}),
                        })
                  }
               };

   // Get the list of device tokens.
   admin.firestore().collection(dev_path+"depiladoras").get()
   .then(result=>
   {  
      const tokens = [];

      result.forEach(function(doc) {
         tokens.push(doc.data().pushToken);
      });

      message.topic="depiladora";
      console.log("se va a enviar")
      const response = admin.messaging().send(message);
      console.log("se envio ok")
      return "ok";
   })
   .catch(function(error) {
      console.log("Error getting documents: ", error);
      throw error;
   });

 
   return "ok";
  });


  

