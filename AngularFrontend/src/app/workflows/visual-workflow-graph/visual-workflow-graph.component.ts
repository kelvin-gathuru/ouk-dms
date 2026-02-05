import { NgClass } from '@angular/common';
import { AfterViewInit, Component, Inject, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { CustomColor } from '@core/domain-classes/custom-color';
import { Link } from '@core/domain-classes/link';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { ClonerService } from '@core/services/clone.service';
import { NgxGraphModule, Node } from '@swimlane/ngx-graph';
import { curveMonotoneX } from 'd3-shape';
import { TranslateModule } from '@ngx-translate/core';
import { WorkflowInstanceStatusPipe } from "../../shared/pipes/workflow-instance-status.pipe";
import { DocumentView } from '@core/domain-classes/document-view';
import { DocumentService } from '../../document/document.service';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-visual-workflow-graph',
  imports: [
    NgxGraphModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    TranslateModule,
    WorkflowInstanceStatusPipe,
    UTCToLocalTime,
    NgClass,
    LimitToPipe,
    MatCardModule
  ],
  templateUrl: './visual-workflow-graph.component.html',
  styleUrl: './visual-workflow-graph.component.scss'
})
export class VisualWorkflowGraphComponent implements AfterViewInit {

  nodes: Node[] = [];
  links: Link[] = [];
  curve = curveMonotoneX; // Choose your curve type
  customColors: CustomColor[] = [];
  @ViewChild('graph') graph: any;
  WORKFLOW_INSTANCE_STATUS = WorkflowInstanceStatus;

  constructor(public dialogRef: MatDialogRef<VisualWorkflowGraphComponent>,
    @Inject(MAT_DIALOG_DATA) public data: VisualWorkflowInstance,
    private clonerService: ClonerService,
    private documentService: DocumentService,
    public overlay: OverlayPanel,) {
    this.nodes = this.clonerService.deepClone<Node[]>(data.nodes);
    this.links = this.clonerService.deepClone<Link[]>(data.links);
    this.customColors = this.clonerService.deepClone<CustomColor[]>(data.customColors);
  }

  onDocumentCancel(): void {
    this.dialogRef.close();
  }

  async onDocumentView() {
    this.documentService.getDocument(this.data.documentId).subscribe(async (document: DocumentInfo) => {
      const urls = document.url?.split('.') ?? [];
      const extension = urls[1];
      const documentView: DocumentView = {
        documentId: document.id,
        name: document.name,
        url: document.url,
        extension: extension,
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isFileRequestDocument: false,
        isSignatureExists: document.isSignatureExists,
        documentNumber: document.documentNumber
      };
      const { BasePreviewComponent } = await import(
        '../../shared/base-preview/base-preview.component'
      );
      this.overlay.open(BasePreviewComponent, {
        position: 'center',
        origin: 'global',
        panelClass: ['file-preview-overlay-container', 'white-background'],
        data: documentView,
      });
    });
  }
  ngAfterViewInit() {
    setTimeout(() => {
      if (this.graph) {
        // this.graph?.zoomToFit(); // Auto zooms to fit view
      }
    });
  }

}
