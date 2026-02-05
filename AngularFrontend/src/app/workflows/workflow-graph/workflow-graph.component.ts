import { Component, Inject } from '@angular/core';
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
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';


@Component({
  selector: 'app-workflow-graph',
  imports: [
    NgxGraphModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    TranslateModule,
    LimitToPipe,
    MatCardModule,
    MatTooltipModule
  ],
  templateUrl: './workflow-graph.component.html',
  styleUrl: './workflow-graph.component.scss'
})
export class WorkflowGraphComponent {
  nodes: Node[] = [];
  links: Link[] = [];
  curve = curveMonotoneX; // Choose your curve type
  customColors: CustomColor[] = [];

  constructor(public dialogRef: MatDialogRef<WorkflowGraphComponent>,
    @Inject(MAT_DIALOG_DATA) public data: VisualWorkflowInstance,
    private clonerService: ClonerService,
    public overlay: OverlayPanel,) {
    this.nodes = this.clonerService.deepClone<Node[]>(data.nodes);
    this.links = this.clonerService.deepClone<Link[]>(data.links);
    this.customColors = this.clonerService.deepClone<CustomColor[]>(data.customColors);
  }

  onDocumentCancel(): void {
    this.dialogRef.close();
  }

}
