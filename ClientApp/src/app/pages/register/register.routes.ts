import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from './register.component';
import { RegisterConfirmEmailComponent } from './components/register-confirm-email.component';

const appRoutes: Routes = [
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'confirm-email',
        component: RegisterConfirmEmailComponent
    }
];

export const RegisterRouting = RouterModule.forChild(appRoutes);
