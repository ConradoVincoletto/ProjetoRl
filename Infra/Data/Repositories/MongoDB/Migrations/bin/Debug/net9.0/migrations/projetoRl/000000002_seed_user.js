var userInsertResult = db.user.insertOne({
    "_id": ObjectId("678bf5233daefa590e05c0e0"),
    "first_name": "ProjetoRl",
    "last_name": "ProjetoRl",
    "email": "contato@projetoRl.com.br",
    "cell_phone": "(19) 99999-9999",
    "state": 1,
    "roles": [0]
});

var userId = userInsertResult.insertedId;

db.passport.insertOne({
    "_id": userId,
    "password": "ObYFTIxuzaN43cV6tR/OA9zdSEAhZfYLnrdpaRXfogs=",
    "secret": "N/x4KJQ5we0uaQ==",
    "type": 0,
    "attempts": 0,
    "state": 1,
    "user_id": ObjectId("678bf5233daefa590e05c0e0")
});