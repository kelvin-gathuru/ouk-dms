import { Component } from '@angular/core';
import { AiDocumentGeneratorFormComponent } from './ai-document-generator-form/ai-document-generator-form.component';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { AiDocumentGeneratorPreviewComponent } from './ai-document-generator-preview/ai-document-generator-preview.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-ai-document-generator',
  imports: [
    RouterModule,
    TranslateModule,
    MatTableModule,
    FormsModule,
    MatIconModule,
    AiDocumentGeneratorPreviewComponent,
    AiDocumentGeneratorFormComponent,
    PageHelpTextComponent,
    MatCardModule
  ],
  templateUrl: './ai-document-generator.component.html',
  styleUrl: './ai-document-generator.component.scss'
})
export class AiDocumentGeneratorComponent {

}
