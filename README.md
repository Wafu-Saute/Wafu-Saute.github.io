# 制作物についての概要説明

　作品名は「萌え家計簿」です。  
　こちらの制作物につきましては、個人で作成し、製作期間は1年ほどで制作致しました。  
キャラクターと触れ合いながら家計簿をつけていくことができるアプリとなります。  
　普通の家計簿にキャラクターとのコミュニケーション要素を加えることで、より家計簿を身近に、つけやすくしたアプリです。  
　まだシステムとしては少ないですが、今後着せ替え要素やキャラクターとのコミュニケーション機能を増やしていく予定です。  
  
　アプリURL: https://wafu-saute.github.io/

【操作説明】  
　・左クリックのみ    
 
 キャラクターをクリックするとランダムでいろんな会話をしてくれます。  
 また、画面下のバーをクリックすることでメニューを表示します。
 
 
 # 開発環境
 
 ・ゲームエンジン：Unity  
 ・開発言語：C#


# 制作意図
　家計簿アプリはたくさんありますが、もう少し起動するのが楽しく、家計簿も楽しくつけていける  
アプリがあればいいなと思い、作成しました。


# 技術的課題や学んだこと
　今回WebGlでビルドし、公開しておりますが、この環境でのデータの保存方法がかなり頭を悩ませました。  
普段はJsonファイル等で保存を行っておりますが、Webでの公開の際は書き出しを行うことが難しく、  
デフォルトで用意されている「PlayerPrefs」する他ありませんでした。  
　また、スマホでの導入も検討している為、タップとクリックを同じロジックを使用して判定できないか検討しました。  
 その結果、一つのメソッド内でクリックの場合とタップの場合とでそれぞれ判定し、結果を文字列で出力することで、  
 一つにまとめることに成功しました。今後は返す文字列を環境変数に変更するなど、なるべくナジックナンバーを減らす  
 よう改修していく必要があります。
