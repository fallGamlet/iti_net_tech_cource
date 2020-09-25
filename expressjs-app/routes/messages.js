const express = require('express')
var router = express.Router();

const messages = [
  {
    "text": "This is a test message"
  }, {
    "text": "Hello world!"
  }

];

router.get('/', (req, res) => {
  res.json(messages)
})

router.post('/', (req, res) => {
  const msg = req.body.text
  if (!msg || typeof(msg) != 'string') {
    res.statusCode = 400
    res.statusMessage = "text field not specified"
    res.json({status:"error", errors:[res.statusMessage]})
    return;
  }

  messages.unshift({"text": msg});
  messages.splice(5);
  res.json({status:"scuccess"})
})

module.exports = router;