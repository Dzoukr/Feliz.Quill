import Quill from 'quill';

const Image = Quill.import('formats/image')

export class ImageBlot extends Image {
    static get ATTRIBUTES() {
        return [ 'alt', 'height', 'width', 'class', 'style' ]
    }

    static formats(domNode) {
        return this.ATTRIBUTES.reduce(function(formats, attribute) {
            if (domNode.hasAttribute(attribute)) {
                formats[attribute] = domNode.getAttribute(attribute);
            }
            return formats;
        }, {});
    }

    format(name, value) {
        if (this.constructor.ATTRIBUTES.indexOf(name) > -1) {
            if (value) {
                this.domNode.setAttribute(name, value);
            } else {
                this.domNode.removeAttribute(name);
            }
        } else {
            super.format(name, value);
        }
    }
}

const Video = Quill.import('formats/video')

export class VideoBlot extends Video {
    static get ATTRIBUTES() {
        return [ 'alt', 'height', 'width', 'class', 'style' ]
    }

    static formats(domNode) {
        return this.ATTRIBUTES.reduce(function(formats, attribute) {
            if (domNode.hasAttribute(attribute)) {
                formats[attribute] = domNode.getAttribute(attribute);
            }
            return formats;
        }, {});
    }

    format(name, value) {
        if (this.constructor.ATTRIBUTES.indexOf(name) > -1) {
            if (value) {
                this.domNode.setAttribute(name, value);
            } else {
                this.domNode.removeAttribute(name);
            }
        } else {
            super.format(name, value);
        }
    }
}
