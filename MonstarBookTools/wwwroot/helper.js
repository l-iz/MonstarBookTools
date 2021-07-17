export function saveCanvas(canvas) {
	//var canvas = document.getElementById(canvas_id);
	//アンカータグを作成
	var a = document.createElement('a');
	//canvasをJPEG変換し、そのBase64文字列をhrefへセット
	a.href = canvas.toDataURL('image/jpeg', 0.85);
	//ダウンロード時のファイル名を指定
	a.download = 'download.jpg';
	//クリックイベントを発生させる
	a.click();
}

export function getElementSize(element_id) {
	var img = document.getElementById(element_id);
	return img.clientWidth + "," + img.clientHeight;
}