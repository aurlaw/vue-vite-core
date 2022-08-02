
function imageUploader(dialog) {
    var image, xhr, xhrComplete, xhrProgress;

    // Set up the event handlers
    dialog.addEventListener('imageuploader.cancelupload', function () {
        // Cancel the current upload

        // Stop the upload
        if (xhr) {
            xhr.upload.removeEventListener('progress', xhrProgress);
            xhr.removeEventListener('readystatechange', xhrComplete);
            xhr.abort();
        }

        // Set the dialog to empty
        dialog.state('empty');
    });
    dialog.addEventListener('imageuploader.clear', function () {
        // Clear the current image
        dialog.clear();
        image = null;
    });
    dialog.addEventListener('imageuploader.rotateccw', function () {
       console.log('not implemented: rotateccw');
    });

    dialog.addEventListener('imageuploader.rotatecw', function () {
        console.log('not implemented: rotatecw');
    });
    dialog.addEventListener('imageuploader.save', function () {
        
        var attr =  {
            'alt': image.alt,
            'data-ce-max-width': image.size[0]
        };
        console.log(attr);
       dialog.save(image.url, image.size, attr);
    });
    dialog.addEventListener('imageuploader.fileready', function (ev) {

        // Upload a file to the server
        var imgSize, blobUrl, blobSasUrl;
        var file = ev.detail().file;

        var sasCookie = getCookie('sastoken');
        var sasData = JSON.parse(sasCookie);

        console.log('sasData', sasData);
        console.log('file', file);

        var img = new Image();
        var _URL = window.URL || window.webkitURL;
        var objectUrl = _URL.createObjectURL(file);
        img.onload = function () {
            console.log(this.width + " " + this.height);
            imgSize = [];
            imgSize.push(this.width);
            imgSize.push(this.height);
            _URL.revokeObjectURL(objectUrl);
        };
        img.src = objectUrl;
        
        blobUrl = sasData.BlobUrl + "/" + file.name;
        blobSasUrl = blobUrl + "?" + sasData.SASToken;
        
        console.log('blobUrl', blobUrl)
            // Define functions to handle upload progress and completion
        xhrProgress = function (ev) {
            // Set the progress for the upload
            dialog.progress((ev.loaded / ev.total) * 100);
        }

        xhrComplete = function (ev) {
            var response;

            console.log('xhrComplete', ev);
            // Check the request is complete
            if (ev.target.readyState !== 4) {
                return;
            }

            // Clear the request
            xhr = null
            xhrProgress = null
            xhrComplete = null

            // Handle the result of the upload
            if (parseInt(ev.target.status) === 200 || parseInt(ev.target.status) === 201) {
                // Unpack the response (from JSON)
                // response = JSON.parse(ev.target.responseText);
                // Store the image details
                image = {
                    size: imgSize,
                    url: blobUrl,
                    alt: file.name
                };

                // Populate the dialog
                dialog.populate(image.url, image.size);

            } else {
                // The request failed, notify the user
                new ContentTools.FlashUI('no');
            }
        }

        // Set the dialog state to uploading and reset the progress bar to 0
        dialog.state('uploading');
        dialog.progress(0);

        // Build the form data to post to the server
        //
        // formData = new FormData();
        // formData.append('image', file);

        // Make the request
        xhr = new XMLHttpRequest();
        xhr.upload.addEventListener('progress', xhrProgress);
        xhr.addEventListener('readystatechange', xhrComplete);
        xhr.open('PUT', blobSasUrl, true);
        xhr.setRequestHeader( "x-ms-blob-type", "BlockBlob");
        xhr.setRequestHeader( "x-ms-meta-expected", file.size);
        xhr.setRequestHeader( "Content-Type", file.type);
        xhr.setRequestHeader( "x-ms-date", new Date().toUTCString());
        
        // var _send = xhr.send;
        // xhr.send = () => { _send.call(xhr, file) };
        
        xhr.send(file);
    });    
    
    
}

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
            'redo',
            'remove'
        ]
    ];
    ContentTools.IMAGE_UPLOADER = imageUploader;
    ContentTools.RESTRICTED_ATTRIBUTES = {
        '*': [],
            'img': ['data-ce-max-width', 'data-ce-min-width'],
            'iframe': ['height', 'width']
    }

    ContentEdit.Root.get().bind('mount', function(elm) {
        // We're only interested in images
        if (elm.type() !== 'Image') { return; }

        // Add an event listener for double clicks
        elm.domElement().addEventListener('dblclick', function(ev) {
            console.log('event', ev);
            console.log('element', elm);

            // replace image
            // TODO: launch modal to choose existing image
            // var image = new ContentEdit.Image({src: 'https://codemicsolutions.blob.core.windows.net/site-media/ventilator-g3f3209fdb_1920.jpg', width: 1920, height: 1279, 'data-ce-max-width': 1920});
            //
            //
            // var imgTool = ContentTools.ToolShelf.fetch('image');// get image tool
            // var insertAt = imgTool._insertAt(elm);// get position for insertion
            // insertAt[0].parent().attach(image, insertAt[1]);// adds new image
            // image.focus();
            // insertAt[0].parent().detach(elm);// removed old image
            

            
        });
    });
    
    
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

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}