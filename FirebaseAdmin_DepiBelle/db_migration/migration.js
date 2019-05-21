//https://firestore.googleapis.com/v1beta1/projects/depi-belle/databases/(default)/documents/offers

const admin = require('../node_modules/firebase-admin');
const serviceAccount = require("./data-transfer-service-key.json");

const data = require("./data.json");

const importPath = "environment/production/"

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: 'https://depi-belle.firebaseio.com'
});

data && Object.keys(data).forEach(key => {
    const nestedContent = data[key];

    if (typeof nestedContent === "object") {
        Object.keys(nestedContent).forEach(index => {
            var id = nestedContent[index].id
            delete nestedContent[index]["id"]
            admin.firestore()
                .collection(importPath+key)
                .doc(id)
                .set(nestedContent[index])
                .then((res) => {
                    console.log("Document successfully written!");
                })
                .catch((error) => {
                    console.error("Error writing document: ", error);
                });
        });
    }
});