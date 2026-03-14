using UnityEngine;
using UnityEngine.UIElements;

namespace Quiz
{
    /// <summary>
    /// A VisualElement that represents a Tooltip. This shows/hides the tooltip and
    /// positions it on-screen.
    /// </summary>
    public class TooltipView : VisualElement
    {
        Label m_TooltipLabel;
        Vector2 m_Offset = new Vector2(60, -60f);

        public Label TooltipLabel { get => m_TooltipLabel; set => m_TooltipLabel = value; }
        public Vector2 Offset { get => m_Offset; set => m_Offset = value; }

        // Define basic setup in Constructor
        public TooltipView()
        {
            m_TooltipLabel = new Label();
            m_TooltipLabel.text = "TOOLTIP_PLACEHOLDER";
            hierarchy.Add(m_TooltipLabel);

            style.position = Position.Absolute;
            pickingMode = PickingMode.Ignore;
            visible = false;
        }

        // Display the tooltip at a specific position with a specified text.
        public void ShowTooltip(VisualElement target, string tooltipText)
        {
            visible = true;

            Vector2 targetPosition = target.worldBound.position;
            Vector2 adjustedPosition = targetPosition + Offset;

            adjustedPosition = ClampToScreen(target, adjustedPosition);

            m_TooltipLabel.style.left = adjustedPosition.x;
            m_TooltipLabel.style.top = adjustedPosition.y;

            m_TooltipLabel.text = tooltipText;
        }

        public void HideTooltip()
        {
            visible = false;
        }

        //  Clamps the position of the tooltip to the screen dimensions.
        private Vector2 ClampToScreen(VisualElement element, Vector2 targetPosition)
        {
            float x = Mathf.Clamp(targetPosition.x, 0, Screen.width - element.layout.width);
            float y = Mathf.Clamp(targetPosition.y, 0, Screen.height - element.layout.height);

            return new Vector2(x, y);
        }
    }

    /// <summary>
    /// An input controller that handles tooltip interaction with mouse events. This is an
    /// example of a Manipulator, which groups several event handlers together into one
    /// class for easier management.
    /// </summary>
    public class TooltipManipulator : MouseManipulator
    {
        // Reference to the custom Tooltip, create this once and reuse it
        TooltipView m_TooltipView;
        readonly int m_DelayMs;
        IVisualElementScheduledItem m_ScheduledShow;

        // Create a new Manipulator for each element
        public TooltipManipulator(TooltipView tooltip, int delayMs)
        {
            m_TooltipView = tooltip;
            m_DelayMs = Mathf.Max(0, delayMs);
        }

        // Register callbacks for mouse enter and leave events.
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        // Unregister callbacks for mouse enter and leave events.
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
            CancelScheduledShow();
        }

        // Method to handle mouse enter events.
        private void OnMouseEnter(MouseEnterEvent evt)
        {
            string tooltipText = target.tooltip;

            if (string.IsNullOrEmpty(tooltipText))
                return;

            CancelScheduledShow();

            m_ScheduledShow = target.schedule.Execute(() =>
            {
                if (target == null)
                    return;

                // Re-check at show time in case state changed during the delay.
                string currentText = target.tooltip;
                if (string.IsNullOrEmpty(currentText))
                    return;

                m_TooltipView.ShowTooltip(target, currentText);
            }).StartingIn(m_DelayMs);
        }

        // Method to handle mouse leave events.
        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            CancelScheduledShow();
            m_TooltipView.HideTooltip();
        }

        void CancelScheduledShow()
        {
            m_ScheduledShow?.Pause();
            m_ScheduledShow = null;
        }
    }

    /// <summary>
    /// Manages the creation and assignment of tooltips.
    /// </summary>
    public class TooltipController
    {
        const string k_StyleSheet = "Uss/Tooltips";
        const string k_TooltipClassName = "tooltip-label";
        const int k_DefaultDelayMs = 400;

        private TooltipView m_TooltipView;
        private VisualElement m_Root;
        private readonly int m_DelayMs;

        public TooltipView TooltipView { get => m_TooltipView; set => m_TooltipView = value; }

        // Constructor
        public TooltipController(VisualElement rootElement, int delayMs = k_DefaultDelayMs)
        {
            m_Root = rootElement;
            m_DelayMs = Mathf.Max(0, delayMs);
            InitializeTooltip();
        }

        // Set starting values and properties
        private void InitializeTooltip()
        {
            TooltipView = new TooltipView();
            m_Root.Add(TooltipView);

            StyleSheet stylesheet = Resources.Load<StyleSheet>(k_StyleSheet);

            if (stylesheet == null)
            {
                Debug.LogError("[TooltipController]: Failed to load Tooltips StyleSheet.");
            }
            else
            {
                TooltipView.TooltipLabel.styleSheets.Add(stylesheet);
                TooltipView.TooltipLabel.AddToClassList(k_TooltipClassName);
            }
        }

        // Assign a tooltip to a specific VisualElement.
        public void AssignTooltipToElement(VisualElement element, string tooltipText)
        {
            if (!string.IsNullOrEmpty(tooltipText))
            {
                // Set the element's tooltip text
                element.tooltip = tooltipText;

                // Use that to create a new TooltipMouseManipulator
                element.AddManipulator(new TooltipManipulator(TooltipView, m_DelayMs));
            }
        }
    }
}
