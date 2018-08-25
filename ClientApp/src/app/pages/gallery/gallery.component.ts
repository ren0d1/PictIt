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
    constructor(private http: HttpClient, private errorHandler: HttpErrorHandlerService){}

    icon: string;
    clicked: boolean;

    searches: SearchDisplay[];

    ngOnInit() {
        this.icon = "image_search";

        this.http.get<SearchDisplay[]>('/api/picture/search').subscribe(result => this.searches = result, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
    }

    fileSelected(files: FileList) {
        this.icon = "cached";
        this.clicked = true;
        for(let i = 0; i < files.length; i++){
            let formData = new FormData();
            formData.append('file', files[i]);

            this.http.post('/api/picture/search', formData).subscribe(() => {}, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
        }
    }

    transferDataSuccess(param: any) {
        let dataTransfer: DataTransfer = param.mouseEvent.dataTransfer;
        if (dataTransfer && dataTransfer.files) { 
            let files: FileList = dataTransfer.files;
            this.icon = "cached";
            this.clicked = true;
            for(let i = 0; i < files.length; i++){
                let formData = new FormData();
                formData.append('file', files[i]);
    
                this.http.post('/api/picture/search', formData).subscribe(() => {}, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
            }
        }
    }
}
