var express = require('express');
var router = express.Router();
var ctrlUsers = require('../login');

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('main', { title: 'CivilMap' });
});

module.exports = router;
