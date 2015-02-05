var express = require('express');
var session = require("express-session");

var path = require('path');
var cors = require('cors');
var compress = require('compression');
var bodyParser = require('body-parser');

var config = require("./config");

var app = express();

app.set('port', config.PORT);

app.use(cors());
app.use(compress());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(express.static(path.join(__dirname, '../client/www'), { maxAge: 2628000000 }));

app.get('/', function(req, res) {
    res.render('index.html');
})

app.listen(app.get('port'), function() {
    console.log('Express server listening on port ' + app.get('port'));
});