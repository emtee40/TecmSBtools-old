/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tsbtool_gui;

import java.awt.Dialog;

/**
 *
 * @author Chris
 */
public class FaceForm extends javax.swing.JDialog {

    /**
     * Creates new form FaceForm
     */
    public FaceForm(Dialog owner, boolean modal) {
        super(owner, modal);
        initComponents();
    }
    private int m_ImageIndex = -1;
    
    /**
     * 
     * @return the image index selected by the user, -1 on cancel.
     */
    public int getImageIndex()
    {
        return m_ImageIndex;
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        mPictureLabel = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
        setIconImage(null);

        mPictureLabel.setIcon(new javax.swing.ImageIcon(getClass().getResource("/tsbtool_gui/facepackage/FaceBackground.png"))); // NOI18N
        mPictureLabel.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mousePressed(java.awt.event.MouseEvent evt) {
                mPictureLabelMousePressed(evt);
            }
        });

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(mPictureLabel)
                .addGap(0, 23, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(mPictureLabel, javax.swing.GroupLayout.PREFERRED_SIZE, 503, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void mPictureLabelMousePressed(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_mPictureLabelMousePressed
        int imageIndex = GetImageIndex(evt.getX(), evt.getY());
        if( imageIndex > -1 )
        {
            m_ImageIndex = imageIndex;
        }
        this.setVisible(false);
    }//GEN-LAST:event_mPictureLabelMousePressed

    private int GetImageIndex(int x, int y)
    {
        int m_Space = 5;
        int m_RaceSpacer = 20;
        int m_ImagesPerRow = 14;
        int height = 32;
        int width = 32;
        
        int ret = -1;
        int col = x /(m_Space +width);
        int row = y /(m_Space + height);
        if( row < 6 )
        {
            ret = row * m_ImagesPerRow + col;
        }
        else
        {
            y -= m_RaceSpacer;
            col = x /(m_Space + width);
            row = y  /(m_Space + height);
            row -= 6;
            ret = row * m_ImagesPerRow + col;
            ret += 0x80;
        }

        if( ret > 0xD4 || (ret > 0x52 && ret < 0x80) )
            ret = -1;
        return ret;
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(FaceForm.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(FaceForm.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(FaceForm.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(FaceForm.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the dialog */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                FaceForm dialog = new FaceForm(new javax.swing.JDialog(), true);
                dialog.addWindowListener(new java.awt.event.WindowAdapter() {
                    @Override
                    public void windowClosing(java.awt.event.WindowEvent e) {
                        System.exit(0);
                    }
                });
                dialog.setVisible(true);
            }
        });
    }
    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JLabel mPictureLabel;
    // End of variables declaration//GEN-END:variables
}
