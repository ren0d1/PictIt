import { Routes, RouterModule } from '@angular/router';

import { ConsentComponent } from './consent.component';

const appRoutes: Routes = [
    {
        path: 'consent',
        component: ConsentComponent
    }
];

export const ConsentRouting = RouterModule.forChild(appRoutes);
