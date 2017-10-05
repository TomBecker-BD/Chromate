var express = require('express');
var path = require('path');
var bodyParser = require('body-parser');
var toaster = require('./api/toaster');
var app = express();
app.set('port', 3000);
app.use(bodyParser.json());
app.use('/api/toaster', toaster);
app.listen(app.get("port"), function() {
    console.log("API server listening on port %d", app.get("port"));
});
