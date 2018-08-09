import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gallery',
  templateUrl: 'gallery.component.html',
  styleUrls: ['gallery.component.css']
})

export class GalleryComponent implements OnInit {

    private image: CatImage = {
      message: 'Progressive Web Cat',
      api: 'https://cataas.com/cat/says/',
      fontsize: 40
    };

    public src: string;

    ngOnInit() {
        this.generateSrc();
    }

    public generateSrc(): void {
        this.src = this.image.api + this.image.message +
          '?size=' + this.image.fontsize +
          '&ts=' + Date.now();
    }
}

class CatImage {
    message: string;
    api: string;
    fontsize: number;
}
