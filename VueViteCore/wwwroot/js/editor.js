window.addEventListener('load', function() {
    var editor;

    ContentTools.StylePalette.add([
        new ContentTools.Style('Note', 'note', ['p']),
        new ContentTools.Style('Note Alt', 'note-alt', ['p']),
        
    ]);
    ContentTools.DEFAULT_TOOLS =  [
        [
            'bold',
            'italic',
            'link',
            'align-left',
            'align-center',
            'align-right'
        ], [
            'paragraph',
            'unordered-list',
            'ordered-list',
            'table',
            'indent',
            'unindent',
            'line-break',
            'image',
        ], [
            'undo',
            'redo'
        ]
    ];
    
    editor = ContentTools.EditorApp.get();
    editor.init('*[data-editable]', 'data-name');
    
    // save
    editor.addEventListener('saved', function (ev) {
        var name, payload, regions, xhr, passive, onStateChange;

        // Check if this was a passive save
        passive = ev.detail().passive;        
        // Check that something changed
        regions = ev.detail().regions;
        if (Object.keys(regions).length === 0) {
            return;
        }
        // Set the editor as busy while we save our changes
        this.busy(true);

        payload = new FormData();
        payload.append('page', document.querySelector('meta[name=editor]').getAttribute('content'));
        // var regionData = {};
        var x = 0;
        for (name in regions) {
            if (regions.hasOwnProperty(name)) {
                // regionData[name] = regions[name];
                payload.append('regions[' + x + '].Key', name );
                payload.append('regions[' + x + '].Value', regions[name] );
                // payload.append(name, regions[name]);
                x++;
            }
        }
        // payload.append('regions', JSON.stringify(regionData));

        // Send the update content to the server to be saved
        onStateChange = function(ev) {
            // Check if the request is finished
            if (ev.target.readyState === 4) {
                editor.busy(false);
                if (ev.target.status === 200 || ev.target.status === 204) {
                    // Save was successful, notify the user with a flash
                    if (!passive) {
                        new ContentTools.FlashUI('ok');
                    }
                } else {
                    // Save failed, notify the user with a flash
                    new ContentTools.FlashUI('no');
                }
            }
        };
        xhr = new XMLHttpRequest();
        xhr.addEventListener('readystatechange', onStateChange);
        xhr.open('POST', '/api/savepage');
        xhr.send(payload);        
        
    });
    
    // // Add support for auto-save
    //     editor.addEventListener('start', function (ev) {
    //         var _this = this;
    //
    //         // Call save every 30 seconds
    //         function autoSave() {
    //             _this.save(true);
    //         };
    //         this.autoSaveTimer = setInterval(autoSave, 30 * 1000);
    //     });
    //
    //     editor.addEventListener('stop', function (ev) {
    //         // Stop the autosave
    //         clearInterval(this.autoSaveTimer);
    //     });    
    
});