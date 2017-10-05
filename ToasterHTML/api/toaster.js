(function () {
    'use strict';
    var express = require('express');
    var router = express.Router();
    router.get('/status', getStatus);
    router.put('/setting', putSetting);
    router.put('/content', putContent);
    router.put('/toasting', putToasting);
    module.exports = router;

    var status = {
        setting: 10,
        content: '',
        toasting: false,
        color: ''
    };

    var statusChanged = false;
    var statusResponse = null;

    var colors = [
        'White', '#EFEBE9', '#D7CCC8', '#BCAAA4', '#A1887F', '#8D6E63', '#795548', '#6D4C41', '#5D4037', '#4E342E',
        '#3E2723', '#212121', 'Black'
    ];

    var simulator = null;

    function getStatus(req, res) {
        if (statusChanged) {
            res.json(status);
            statusChanged = false;
        } else {
            let msec = Number(req.get('timeout') || 0) * 1000;
            statusResponse = res;
            setTimeout(() => {
                if (statusResponse) {
                    statusResponse.json(status);
                    statusResponse = null;
                }
            }, msec);
        }
    }

    function sendStatus() {
        if (statusResponse) {
            statusResponse.json(status);
            statusResponse = null;
            statusChanged = false;
        } else {
            statusChanged = true;
        }
    }

    function putSetting(req, res) {
        status.setting = req.body.setting;
        res.json(status);
    }

    function putContent(req, res) {
        if (status.toasting) {
            res.status(400).send('Cannot put toaster content while toasting');
            return;
        }
        status.content = req.body.content;
        status.color = req.body.color || colors[0];
        res.json(status);
    }

    function putToasting(req, res) {
        if (status.toasting === req.body.toasting) {
            if (status.toasting) {
                res.status(400).send('Already toasting');
            } else {
                res.status(400).send('Already not toasting');
            }
            return;
        }

        if (req.body.toasting) {
            start();
        } else {
            stop();
        }

        res.json(status);
    }

    function start() {
        status.toasting = true;
        sendStatus();
        let time = 0;
        let rate = 0.5;
        let heat = 1;
        let doneness = Math.max(0, colors.indexOf(status.color));
        simulator = setInterval(() => {
            time += rate;
            doneness += heat;
            let colorIndex = Math.min(Math.floor(doneness), colors.length - 1);
            status.color = colors[colorIndex];
            if (time >= status.setting) {
                clearInterval(simulator);
                simulator = null;
                status.toasting = false;
            }
            sendStatus();
        }, rate * 1000);
    }

    function stop() {
        if (simulator) {
            clearInterval(simulator);
            simulator = null;
        }
        status.toasting = false;
        sendStatus();
    }
})();
