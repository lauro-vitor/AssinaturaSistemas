const loading = {
    bloquear: function () {
        $('body').waitMe({
            effect: 'roundBounce',
            text: 'Loading ...',
            bg: 'rgba(255, 255, 255, 0.9)',
            color: '#000',
            maxSize: '',
            waitTime: -1,
            textPos: 'vertical',
            fontSize: '',
            source: '',
            onClose: function () { }
        });
    },
    desbloquear: function () {
        $('body').waitMe('hide');
    }
}