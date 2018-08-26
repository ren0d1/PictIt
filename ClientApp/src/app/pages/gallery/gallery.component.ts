import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpErrorHandlerService } from '../../shared/services/http-error-handler.service';
import { SearchDisplay } from '../../shared/models/search-display.model';

@Component({
  selector: 'app-gallery',
  templateUrl: 'gallery.component.html',
  styleUrls: ['gallery.component.css']
})

export class GalleryComponent implements OnInit {
    constructor(private http: HttpClient, private errorHandler: HttpErrorHandlerService) {}

    icon: string;
    clicked: boolean;

    searches: SearchDisplay[];

    ngOnInit() {
        this.icon = 'image_search';

        this.http.get<SearchDisplay[]>('/api/picture/search').subscribe(result => this.searches = result, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
    }

    fileSelected(files: FileList) {
        this.icon = 'cached';
        this.clicked = true;
        for (let i = 0; i < files.length; i++) {
            const formData = new FormData();
            formData.append('file', files[i]);

            this.http.post('/api/picture/search', formData).subscribe(() => {
                location.reload();
            }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
        }
    }

    transferDataSuccess(param: any) {
        const dataTransfer: DataTransfer = param.mouseEvent.dataTransfer;
        if (dataTransfer && dataTransfer.files) {
            const files: FileList = dataTransfer.files;
            this.icon = 'cached';
            this.clicked = true;
            for (let i = 0; i < files.length; i++) {
                const formData = new FormData();
                formData.append('file', files[i]);

                this.http.post('/api/picture/search', formData).subscribe(() => {
                    location.reload();
                }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
            }
        }
    }

    getEmotionEmoji(emotion: string) {
        switch (emotion) {
            case 'anger':
                return '😡';
            case 'contempt':
                return '🙄';
            case 'disgust':
                return '🤢';
            case 'fear':
                return '😨';
            case 'happiness':
                return '😁';
            case 'neutral':
                return '😐';
            case 'sadness':
                return '😭';
            case 'surprise':
                return '😲';
        }
    }
}
