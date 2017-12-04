var express = require('express');
var router = express.Router();


router.get('/checkToken', ctrlUsers.checkUser);
router.post('/login', ctrlUsers.login);
router.get('/logout', ctrlUsers.logout);

router.post('/', router);
module.exports = router;