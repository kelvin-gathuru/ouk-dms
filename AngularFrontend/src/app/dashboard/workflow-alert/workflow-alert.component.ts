import { Component, inject } from '@angular/core';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-workflow-alert',
    standalone: true,
    imports: [MatDialogModule, MatButtonModule, MatIconModule, TranslateModule],
    templateUrl: './workflow-alert.component.html',
    styleUrls: ['./workflow-alert.component.scss']
})
export class WorkflowAlertComponent {
    private dialogRef = inject(MatDialogRef<WorkflowAlertComponent>);
    private router = inject(Router);

    onViewWorkflows() {
        this.router.navigate(['/current-workflow']);
        this.dialogRef.close();
    }

    onClose() {
        this.dialogRef.close();
    }
}
